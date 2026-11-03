using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using AirQualityMLP.Data;

namespace AirQualityMLP.Training
{
    /// <summary>One cell of the grid search.</summary>
    public class TuningResult
    {
        public TrainingConfig Config;
        public double[] ValRmseAtCheckpoint;   // validation RMSE (ug/m3) after the checkpoint epochs
        public int BestEpoch;
        public double BestValRmse;
        public double FinalTrainMse;
        public bool Diverged;
        public double Seconds;
    }

    /// <summary>
    /// Grid search over learning rate x hidden nodes x input transform. Every configuration is trained for the
    /// full epoch budget WITHOUT early stopping so that the validation curve shows how many epochs are useful;
    /// the validation RMSE after 50 / 100 / 200 / 400 epochs and the best epoch are recorded.
    /// Only the validation set is used here - the test set stays untouched.
    /// </summary>
    public class HyperparameterTuner
    {
        /// <summary>Called after every finished configuration (used to fill the table live).</summary>
        public delegate void TuningProgress(int done, int total, TuningResult result);

        public static readonly int[] Checkpoints = { 50, 100, 200, 400 };

        public static List<TuningResult> Run(List<AirQualityRecord> records, DataSplit split,
                                             double[] learningRates, int[] hiddenSizes, bool[] logOptions,
                                             int maxEpochs, int seed, TextWriter console)
        {
            return Run(records, split, learningRates, hiddenSizes, logOptions, maxEpochs, seed, console, null, null);
        }

        public static List<TuningResult> Run(List<AirQualityRecord> records, DataSplit split,
                                             double[] learningRates, int[] hiddenSizes, bool[] logOptions,
                                             int maxEpochs, int seed, TextWriter console,
                                             MlpRegressor.StopCheck stop, TuningProgress progress)
        {
            List<TuningResult> results = new List<TuningResult>();
            int total = learningRates.Length * hiddenSizes.Length * logOptions.Length, done = 0;

            foreach (bool log in logOptions)
            {
                Preprocessor pre = new Preprocessor(log);
                pre.Fit(records, split.Train);
                ScaledSet train = pre.Transform(records, split.Train);
                ScaledSet val = pre.Transform(records, split.Validation);

                foreach (int hidden in hiddenSizes)
                    foreach (double lr in learningRates)
                    {
                        TrainingConfig cfg = new TrainingConfig();
                        cfg.LearningRate = lr; cfg.HiddenNodes = hidden; cfg.LogTransform = log;
                        cfg.MaxEpochs = maxEpochs; cfg.Patience = maxEpochs + 1; cfg.Seed = seed;

                        if (stop != null && stop()) return results;
                        MlpRegressor model = new MlpRegressor(pre.InputCount, cfg, pre);
                        model.ShouldStop = stop;
                        TrainingResult tr = model.Train(train, val, null);
                        if (tr.Cancelled) return results;

                        TuningResult r = new TuningResult();
                        r.Config = cfg;
                        r.BestEpoch = tr.BestEpoch;
                        r.BestValRmse = tr.BestValRmse;
                        r.Diverged = tr.Diverged;
                        r.Seconds = tr.Seconds;
                        r.FinalTrainMse = tr.History.Count > 0 ? tr.History[tr.History.Count - 1].TrainMse : double.NaN;
                        r.ValRmseAtCheckpoint = new double[Checkpoints.Length];
                        for (int c = 0; c < Checkpoints.Length; c++)
                            r.ValRmseAtCheckpoint[c] = Checkpoints[c] <= tr.History.Count
                                ? tr.History[Checkpoints[c] - 1].ValRmse : double.NaN;
                        results.Add(r);

                        done++;
                        if (progress != null) progress(done, total, r);
                        console.WriteLine("[{0,2}/{1}] log={2,-5} hidden={3,-3} lr={4,-5} -> best val RMSE {5,8:F3} ug/m3 at epoch {6,3}{7}  ({8:F1}s)",
                            done, total, log, hidden, lr,
                            tr.Diverged ? double.NaN : tr.BestValRmse, tr.BestEpoch, tr.Diverged ? "  DIVERGED" : "", tr.Seconds);
                    }
            }
            return results;
        }

        public static TuningResult Best(List<TuningResult> results)
        {
            TuningResult best = null;
            foreach (TuningResult r in results)
                if (!r.Diverged && (best == null || r.BestValRmse < best.BestValRmse)) best = r;
            return best;
        }

        public static void WriteCsv(string path, List<TuningResult> results)
        {
            using (StreamWriter w = new StreamWriter(path))
            {
                w.Write("log_transform,hidden_nodes,learning_rate,best_epoch,best_val_rmse");
                foreach (int c in Checkpoints) w.Write(",val_rmse_epoch_" + c);
                w.WriteLine(",final_train_mse,diverged,seconds");
                foreach (TuningResult r in results)
                {
                    w.Write(string.Format(CultureInfo.InvariantCulture, "{0},{1},{2},{3},{4:F4}",
                        r.Config.LogTransform, r.Config.HiddenNodes, r.Config.LearningRate, r.BestEpoch,
                        r.Diverged ? double.NaN : r.BestValRmse));
                    foreach (double v in r.ValRmseAtCheckpoint)
                        w.Write(string.Format(CultureInfo.InvariantCulture, ",{0:F4}", v));
                    w.WriteLine(string.Format(CultureInfo.InvariantCulture, ",{0:F6},{1},{2:F1}", r.FinalTrainMse, r.Diverged, r.Seconds));
                }
            }
        }
    }
}
