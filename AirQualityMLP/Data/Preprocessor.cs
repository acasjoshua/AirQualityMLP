using System;
using System.Collections.Generic;

namespace AirQualityMLP.Data
{
    /// <summary>A subset ready for the network: scaled inputs, scaled target and the original target.</summary>
    public class ScaledSet
    {
        public double[][] X;          // [row][feature], roughly 0..1
        public double[] YScaled;      // target squeezed into [TargetLow, TargetHigh]
        public double[] YRaw;         // PM2.5 in ug/m3 (for metrics in real units)
        public int[] RecordIndex;     // which record each row came from
    }

    /// <summary>
    /// Feature engineering + min-max scaling. Statistics (min / max) are learned from the TRAINING rows only
    /// and then applied unchanged to validation and test rows, so no information leaks from the held-out data.
    ///
    /// Inputs (13):  11 pollutants (optionally log1p-transformed, then min-max scaled to [0,1])
    ///               + hour-of-day as sin / cos (so 23:00 and 00:00 are neighbours), mapped from [-1,1] to [0,1].
    /// Target:       PM2.5 (optionally log1p-transformed), min-max scaled into [0.1, 0.9] because the output
    ///               neuron is a sigmoid and can never reach exactly 0 or 1.
    /// </summary>
    public class Preprocessor
    {
        public const double TargetLow = 0.1;
        public const double TargetHigh = 0.9;

        private readonly bool logTransform;
        private double[] featMin, featMax;
        private double targetMin, targetMax;
        private bool fitted;

        public Preprocessor(bool logTransform)
        {
            this.logTransform = logTransform;
        }

        public bool LogTransform { get { return logTransform; } }
        public int InputCount { get { return featMin.Length + 2; } }

        public static List<string> InputNames(List<string> pollutantNames)
        {
            List<string> names = new List<string>(pollutantNames);
            names.Add("hour_sin");
            names.Add("hour_cos");
            return names;
        }

        private double Tf(double v) { return logTransform ? Math.Log(1.0 + v) : v; }
        private double InvTf(double v) { return logTransform ? Math.Exp(v) - 1.0 : v; }

        /// <summary>Learns min / max from the training rows only.</summary>
        public void Fit(List<AirQualityRecord> records, int[] trainIndex)
        {
            int nFeat = records[0].Pollutants.Length;
            featMin = new double[nFeat];
            featMax = new double[nFeat];
            for (int k = 0; k < nFeat; k++) { featMin[k] = double.MaxValue; featMax[k] = double.MinValue; }
            targetMin = double.MaxValue; targetMax = double.MinValue;

            foreach (int i in trainIndex)
            {
                AirQualityRecord r = records[i];
                for (int k = 0; k < nFeat; k++)
                {
                    double v = Tf(r.Pollutants[k]);
                    if (v < featMin[k]) featMin[k] = v;
                    if (v > featMax[k]) featMax[k] = v;
                }
                double t = Tf(r.Pm25);
                if (t < targetMin) targetMin = t;
                if (t > targetMax) targetMax = t;
            }
            fitted = true;
        }

        public double[] Features(AirQualityRecord r)
        {
            if (!fitted) throw new InvalidOperationException("Call Fit first.");
            int nFeat = featMin.Length;
            double[] x = new double[nFeat + 2];
            for (int k = 0; k < nFeat; k++)
            {
                double range = featMax[k] - featMin[k];
                // Values outside the training range are NOT clipped: the hidden sigmoids can handle a little extrapolation.
                x[k] = range > 1e-12 ? (Tf(r.Pollutants[k]) - featMin[k]) / range : 0.5;
            }
            double angle = 2.0 * Math.PI * r.Time.Hour / 24.0;
            x[nFeat] = (Math.Sin(angle) + 1.0) / 2.0;
            x[nFeat + 1] = (Math.Cos(angle) + 1.0) / 2.0;
            return x;
        }

        public double ScaleTarget(double pm25)
        {
            double range = targetMax - targetMin;
            double unit = range > 1e-12 ? (Tf(pm25) - targetMin) / range : 0.5;
            return TargetLow + unit * (TargetHigh - TargetLow);
        }

        /// <summary>Network output (0..1) back to ug/m3. Never returns a negative concentration.</summary>
        public double InverseTarget(double scaled)
        {
            double unit = (scaled - TargetLow) / (TargetHigh - TargetLow);
            double v = InvTf(targetMin + unit * (targetMax - targetMin));
            return v < 0 ? 0 : v;
        }

        public ScaledSet Transform(List<AirQualityRecord> records, int[] index)
        {
            ScaledSet s = new ScaledSet();
            s.X = new double[index.Length][];
            s.YScaled = new double[index.Length];
            s.YRaw = new double[index.Length];
            s.RecordIndex = index;
            for (int n = 0; n < index.Length; n++)
            {
                AirQualityRecord r = records[index[n]];
                s.X[n] = Features(r);
                s.YRaw[n] = r.Pm25;
                s.YScaled[n] = ScaleTarget(r.Pm25);
            }
            return s;
        }
    }
}
