using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AirQualityMLP.Data;
using Backprop;

namespace AirQualityMLP.Training
{
    /// <summary>
    /// Wraps the Backprop.NeuralNet (input / hidden / output classes) for a single-output regression task:
    /// builds the network, runs the epoch loop, keeps the weights of the best validation epoch.
    /// </summary>
    public class MlpRegressor
    {
        public delegate void EpochLogger(EpochRecord record);

        /// <summary>Polled after every epoch; return true to stop training early (used by the Stop button).</summary>
        public delegate bool StopCheck();

        public StopCheck ShouldStop;

        private readonly NeuralNet net;
        private readonly Preprocessor pre;
        private readonly TrainingConfig config;
        private string bestWeights;          // text snapshot of the best epoch

        public MlpRegressor(int inputCount, TrainingConfig config, Preprocessor pre)
        {
            this.config = config;
            this.pre = pre;
            NeuralNet.SetSeed(config.Seed);                       // reproducible initial weights
            net = new NeuralNet(inputCount, config.HiddenNodes, 1);
            net.LearningRateIn = config.LearningRate;
            net.LearningRateOut = config.LearningRate;
        }

        /// <summary>Raw network output (0..1 scale).</summary>
        public double PredictScaled(double[] x)
        {
            for (int i = 0; i < x.Length; i++) net.setInputs(i, x[i]);
            net.run();
            return net.getOuputData(0);
        }

        /// <summary>Prediction in ug/m3.</summary>
        public double Predict(double[] x)
        {
            return pre.InverseTarget(PredictScaled(x));
        }

        public double[] PredictAll(double[][] x)
        {
            double[] p = new double[x.Length];
            for (int i = 0; i < x.Length; i++) p[i] = Predict(x[i]);
            return p;
        }

        /// <summary>
        /// Online (one sample at a time) back-propagation. After every epoch the validation set is scored;
        /// the best epoch's weights are remembered and restored at the end (early stopping).
        /// </summary>
        public TrainingResult Train(ScaledSet train, ScaledSet validation, EpochLogger logger)
        {
            TrainingResult result = new TrainingResult();
            Stopwatch clock = Stopwatch.StartNew();
            Random shuffler = new Random(config.Seed + 1000);

            int n = train.X.Length;
            int[] order = new int[n];
            for (int i = 0; i < n; i++) order[i] = i;

            int sinceBest = 0;
            for (int epoch = 1; epoch <= config.MaxEpochs; epoch++)
            {
                for (int i = n - 1; i > 0; i--)               // new sample order every epoch
                {
                    int j = shuffler.Next(i + 1);
                    int t = order[i]; order[i] = order[j]; order[j] = t;
                }

                double sse = 0;
                foreach (int s in order)
                {
                    double[] x = train.X[s];
                    for (int k = 0; k < x.Length; k++) net.setInputs(k, x[k]);
                    net.setDesiredOutput(0, train.YScaled[s]);
                    sse += 2.0 * net.learn();                 // learn() returns 1/2 * squared error
                }

                EpochRecord rec = new EpochRecord();
                rec.Epoch = epoch;
                rec.TrainMse = sse / n;
                Validate(validation, out rec.ValMse, out rec.ValRmse);
                result.History.Add(rec);
                if (logger != null) logger(rec);

                if (ShouldStop != null && ShouldStop()) { result.Cancelled = true; break; }

                if (double.IsNaN(rec.TrainMse) || double.IsNaN(rec.ValRmse) || rec.TrainMse > 10)
                {
                    result.Diverged = true;                   // learning rate too high: weights blew up
                    break;
                }

                if (rec.ValRmse < result.BestValRmse - 1e-9)
                {
                    result.BestValRmse = rec.ValRmse;
                    result.BestEpoch = epoch;
                    sinceBest = 0;
                    StringWriter w = new StringWriter();
                    net.saveWeights(w);
                    bestWeights = w.ToString();
                }
                else if (++sinceBest >= config.Patience) break;
            }

            if (bestWeights != null) net.loadWeights(new StringReader(bestWeights));   // restore the best epoch
            result.Seconds = clock.Elapsed.TotalSeconds;
            return result;
        }

        private void Validate(ScaledSet set, out double mseScaled, out double rmseReal)
        {
            double sseScaled = 0, sseReal = 0;
            for (int i = 0; i < set.X.Length; i++)
            {
                double p = PredictScaled(set.X[i]);
                double d = p - set.YScaled[i];
                sseScaled += d * d;
                double e = pre.InverseTarget(p) - set.YRaw[i];
                sseReal += e * e;
            }
            mseScaled = sseScaled / set.X.Length;
            rmseReal = Math.Sqrt(sseReal / set.X.Length);
        }
    }
}
