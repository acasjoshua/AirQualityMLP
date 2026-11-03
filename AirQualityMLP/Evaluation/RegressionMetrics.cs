using System;

namespace AirQualityMLP.Evaluation
{
    /// <summary>Standard regression scores, all in the original units (ug/m3).</summary>
    public class RegressionMetrics
    {
        public int Count;
        public double Mse, Rmse, Mae, R2, Bias;

        /// <summary>Bias = mean(predicted - actual): positive means the model over-predicts.</summary>
        public static RegressionMetrics Compute(double[] actual, double[] predicted)
        {
            int n = actual.Length;
            double mean = 0;
            for (int i = 0; i < n; i++) mean += actual[i];
            mean /= n;

            double sse = 0, sae = 0, sst = 0, bias = 0;
            for (int i = 0; i < n; i++)
            {
                double e = predicted[i] - actual[i];
                sse += e * e;
                sae += Math.Abs(e);
                bias += e;
                sst += (actual[i] - mean) * (actual[i] - mean);
            }
            RegressionMetrics m = new RegressionMetrics();
            m.Count = n;
            m.Mse = sse / n;
            m.Rmse = Math.Sqrt(m.Mse);
            m.Mae = sae / n;
            m.Bias = bias / n;
            m.R2 = sst > 0 ? 1.0 - sse / sst : 0;
            return m;
        }

        public override string ToString()
        {
            return string.Format("MSE={0:F2}  RMSE={1:F2}  MAE={2:F2}  R2={3:F4}  bias={4:F2}", Mse, Rmse, Mae, R2, Bias);
        }
    }

    /// <summary>MAE / RMSE / bias for the hours whose TRUE PM2.5 lies in one air-quality band.</summary>
    public class BandStat
    {
        public string Name;
        public int Count;
        public double Mae, Rmse, Bias;

        public static System.Collections.Generic.List<BandStat> Compute(double[] actual, double[] predicted)
        {
            System.Collections.Generic.List<BandStat> list = new System.Collections.Generic.List<BandStat>();
            for (int band = 0; band < AqiCategory.Names.Length; band++)
            {
                System.Collections.Generic.List<double> a = new System.Collections.Generic.List<double>();
                System.Collections.Generic.List<double> p = new System.Collections.Generic.List<double>();
                for (int i = 0; i < actual.Length; i++)
                    if (AqiCategory.FromPm25(actual[i]) == band) { a.Add(actual[i]); p.Add(predicted[i]); }
                if (a.Count == 0) continue;
                RegressionMetrics m = RegressionMetrics.Compute(a.ToArray(), p.ToArray());
                BandStat b = new BandStat();
                b.Name = AqiCategory.Names[band]; b.Count = a.Count; b.Mae = m.Mae; b.Rmse = m.Rmse; b.Bias = m.Bias;
                list.Add(b);
            }
            return list;
        }
    }
}
