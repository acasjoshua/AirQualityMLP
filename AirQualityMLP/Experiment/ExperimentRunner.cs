using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using AirQualityMLP.Data;
using AirQualityMLP.Evaluation;
using AirQualityMLP.Training;

namespace AirQualityMLP.Experiment
{
    public class ExperimentOptions
    {
        public string DataPath;
        public string OutDir;
        public bool Quick;
    }

    /// <summary>One row of the data summary table.</summary>
    public class ColumnStat
    {
        public string Name;
        public double Min, Mean, Median, Max, CorrelationWithTarget;
    }

    public class DatasetInfo
    {
        public string Path;
        public LoadReport Report;
        public DateTime First, Last;
        public int TrainRows, ValidationRows, TestRows;
        public List<ColumnStat> Columns = new List<ColumnStat>();   // PM2.5 first, then the predictors
    }

    /// <summary>Everything the window needs to show after a run (and to answer "what would the model predict for ...?").</summary>
    public class ExperimentResult
    {
        public bool Cancelled;
        public string OutDir;
        public List<AirQualityRecord> Records;
        public DataSplit Split;
        public Preprocessor Pre;
        public MlpRegressor Model;                  // the primary (seed 1) network
        public TrainingConfig Config;               // hyperparameters of the final network
        public List<TuningResult> Grid;
        public TuningResult Best;
        public TrainingResult PrimaryTrain;
        public List<RegressionMetrics> MlpMetrics = new List<RegressionMetrics>();
        public List<double> BandAccuracies = new List<double>();
        public RegressionMetrics MeanBaseline, LinearBaseline;
        public double[] TestActual, TestPredicted;  // primary model, original units
        public int[] TestRecordIndex;               // which record each test row is
        public int[,] Confusion;
        public List<BandStat> Bands;
        public double BandAccuracy, WithinOneBand;

        /// <summary>PM2.5 (ug/m3) predicted by the primary network for the given pollutant readings and hour.</summary>
        public double Predict(double[] pollutants, int hour)
        {
            AirQualityRecord r = new AirQualityRecord();
            r.Pollutants = pollutants;
            r.Time = new DateTime(2020, 1, 1, hour, 0, 0);
            return Model.Predict(Pre.Features(r));
        }
    }

    /// <summary>Optional notifications for a window; every callback runs on the thread that calls Run.</summary>
    public class ExperimentSink
    {
        public Action<string> Stage;                              // short text such as "Hyperparameter tuning"
        public Action<DatasetInfo> DataLoaded;
        public HyperparameterTuner.TuningProgress TuningProgress;
        public Action<TuningResult> BestChosen;
        public delegate void EpochHandler(int seed, int maxEpochs, EpochRecord record);
        public delegate void SeedDoneHandler(int seed, int seedCount, TrainingResult training, RegressionMetrics test);

        public EpochHandler Epoch;
        public SeedDoneHandler SeedDone;
        public MlpRegressor.StopCheck ShouldStop;
    }

    /// <summary>
    /// The whole experiment (gradient check, data, grid search, final training, evaluation, result files).
    /// Used by both the window (Form1) and the command line.
    /// </summary>
    public static class ExperimentRunner
    {
        private const int SplitSeed = 42;
        private const int FinalSeeds = 5;

        public static ExperimentResult Run(ExperimentOptions options, TextWriter log, ExperimentSink sink)
        {
            if (sink == null) sink = new ExperimentSink();
            string outDir = options.OutDir;
            Directory.CreateDirectory(outDir);
            ExperimentResult res = new ExperimentResult();
            res.OutDir = outDir;
            bool quick = options.Quick;

            log.WriteLine("=== PM2.5 regression with a custom back-propagation MLP ===");
            log.WriteLine("Mode: " + (quick ? "QUICK (smoke test)" : "FULL"));
            log.WriteLine();

            // 1. correctness of back-propagation ---------------------------------------------------------------
            Stage(sink, "Checking back-propagation (gradient check)");
            log.WriteLine("--- 1. Back-propagation gradient check ---");
            bool gradOk = GradientCheck.Run(log);
            log.WriteLine();
            if (!gradOk) { log.WriteLine("Gradient check FAILED - aborting."); res.Cancelled = true; return res; }

            // 2. data ---------------------------------------------------------------------------------------------
            Stage(sink, "Loading data");
            log.WriteLine("--- 2. Data ---");
            LoadReport report;
            List<AirQualityRecord> records = CsvLoader.Load(options.DataPath, out report);
            log.WriteLine("File: " + options.DataPath);
            log.WriteLine("Lines read: {0}, rows kept: {1}, dropped: malformed={2}, bad date={3}, missing/NaN={4}, negative={5}",
                report.DataLines, report.Loaded, report.DroppedMalformed, report.DroppedBadDate, report.DroppedMissing, report.DroppedNegative);
            DateTime first = DateTime.MaxValue, last = DateTime.MinValue;
            foreach (AirQualityRecord r in records) { if (r.Time < first) first = r.Time; if (r.Time > last) last = r.Time; }
            log.WriteLine("Period: {0:yyyy-MM-dd HH:mm} to {1:yyyy-MM-dd HH:mm}", first, last);
            log.WriteLine("Target: {0};  predictors: {1}  + hour_sin, hour_cos", CsvLoader.TargetColumn, string.Join(", ", CsvLoader.FeatureColumns.ToArray()));
            log.WriteLine();

            DataSplit split = DataSplit.Create(records.Count, 0.70, 0.15, SplitSeed);
            log.WriteLine("Split (seed {0}): train={1}  validation={2}  test={3}", SplitSeed, split.Train.Length, split.Validation.Length, split.Test.Length);
            log.WriteLine();
            res.Records = records;
            res.Split = split;

            if (sink.DataLoaded != null)
                sink.DataLoaded(Describe(options.DataPath, records, report, first, last, split));

            // 3. hyperparameter search -----------------------------------------------------------------------------
            Stage(sink, "Hyperparameter tuning");
            log.WriteLine("--- 3. Hyperparameter tuning (validation set only) ---");
            double[] lrs = quick ? new double[] { 0.1, 0.5 } : new double[] { 0.05, 0.1, 0.2, 0.5, 1.0, 2.0, 4.0 };
            int[] hiddens = quick ? new int[] { 8, 16 } : new int[] { 4, 8, 16, 32, 64 };
            int gridEpochs = quick ? 30 : 400;
            List<TuningResult> grid = HyperparameterTuner.Run(records, split, lrs, hiddens, new bool[] { false, true }, gridEpochs, 1,
                                                              log, sink.ShouldStop, sink.TuningProgress);
            res.Grid = grid;
            if (Stopped(sink)) { res.Cancelled = true; return res; }

            HyperparameterTuner.WriteCsv(Path.Combine(outDir, "tuning_results.csv"), grid);
            WriteTuningPlots(outDir, grid, lrs, hiddens);

            TuningResult best = HyperparameterTuner.Best(grid);
            if (best == null) { log.WriteLine("Every configuration diverged."); res.Cancelled = true; return res; }
            res.Best = best;
            if (sink.BestChosen != null) sink.BestChosen(best);
            log.WriteLine();
            log.WriteLine("Best on validation: {0}  (val RMSE {1:F3} ug/m3 at epoch {2})", best.Config, best.BestValRmse, best.BestEpoch);
            log.WriteLine();

            // 4. final training -------------------------------------------------------------------------------------
            Stage(sink, "Final training");
            log.WriteLine("--- 4. Final training with the chosen hyperparameters ---");
            Preprocessor pre = new Preprocessor(best.Config.LogTransform);
            pre.Fit(records, split.Train);
            ScaledSet train = pre.Transform(records, split.Train);
            ScaledSet val = pre.Transform(records, split.Validation);
            ScaledSet test = pre.Transform(records, split.Test);
            List<string> inputNames = Preprocessor.InputNames(CsvLoader.FeatureColumns);
            res.Pre = pre;

            int seeds = quick ? 2 : FinalSeeds;
            List<int> bestEpochs = new List<int>();
            int maxEpochs = quick ? 40 : 1000;

            for (int seed = 1; seed <= seeds; seed++)
            {
                TrainingConfig cfg = new TrainingConfig();
                cfg.LearningRate = best.Config.LearningRate;
                cfg.HiddenNodes = best.Config.HiddenNodes;
                cfg.LogTransform = best.Config.LogTransform;
                cfg.MaxEpochs = maxEpochs;
                cfg.Patience = 50;
                cfg.Seed = seed;

                MlpRegressor model = new MlpRegressor(pre.InputCount, cfg, pre);
                model.ShouldStop = sink.ShouldStop;
                bool primary = (seed == 1);
                int thisSeed = seed;
                if (primary) log.WriteLine("Seed 1 (primary model) - epoch log, every 10th epoch:");
                if (primary) log.WriteLine("{0,6}{1,14}{2,14}{3,16}", "epoch", "train MSE", "val MSE", "val RMSE ug/m3");

                TrainingResult tr = model.Train(train, val, delegate(EpochRecord e)
                {
                    if (primary && (e.Epoch == 1 || e.Epoch % 10 == 0))
                        log.WriteLine("{0,6}{1,14:F6}{2,14:F6}{3,16:F3}", e.Epoch, e.TrainMse, e.ValMse, e.ValRmse);
                    if (sink.Epoch != null) sink.Epoch(thisSeed, maxEpochs, e);
                });
                if (tr.Cancelled) { res.Cancelled = true; return res; }
                WriteTrainingLog(Path.Combine(outDir, primary ? "training_log.csv" : "training_log_seed" + seed + ".csv"), tr);

                double[] pred = model.PredictAll(test.X);
                RegressionMetrics m = RegressionMetrics.Compute(test.YRaw, pred);
                res.MlpMetrics.Add(m);
                res.BandAccuracies.Add(AqiCategory.Accuracy(test.YRaw, pred));
                bestEpochs.Add(tr.BestEpoch);
                log.WriteLine("Seed {0}: stopped after {1} epochs (best epoch {2}, val RMSE {3:F3}) in {4:F1}s   TEST {5}",
                    seed, tr.History.Count, tr.BestEpoch, tr.BestValRmse, tr.Seconds, m);
                if (primary)
                {
                    res.Model = model; res.Config = cfg; res.PrimaryTrain = tr;
                    res.TestPredicted = pred; res.TestActual = test.YRaw; res.TestRecordIndex = test.RecordIndex;
                }
                if (sink.SeedDone != null) sink.SeedDone(seed, seeds, tr, m);
            }
            log.WriteLine();

            // 5. evaluation -----------------------------------------------------------------------------------------
            Stage(sink, "Evaluating on the test set");
            log.WriteLine("--- 5. Evaluation on the untouched test set ---");
            double meanTrain = 0;
            foreach (double v in train.YRaw) meanTrain += v;
            meanTrain /= train.YRaw.Length;
            double[] meanPred = new double[test.YRaw.Length];
            for (int i = 0; i < meanPred.Length; i++) meanPred[i] = meanTrain;
            res.MeanBaseline = RegressionMetrics.Compute(test.YRaw, meanPred);

            LinearBaseline ols = new LinearBaseline(pre);
            ols.Fit(train);
            res.LinearBaseline = RegressionMetrics.Compute(test.YRaw, ols.Predict(test));

            log.WriteLine("{0,-34}{1}", "Predict-the-mean baseline:", res.MeanBaseline);
            log.WriteLine("{0,-34}{1}", "Linear regression (reference):", res.LinearBaseline);
            log.WriteLine("{0,-34}{1}", "MLP, seed 1 (primary):", res.MlpMetrics[0]);
            log.WriteLine("MLP over {0} seeds: RMSE {1}, MAE {2}, R2 {3}", seeds,
                MeanStd(res.MlpMetrics, delegate(RegressionMetrics m) { return m.Rmse; }, "F2"),
                MeanStd(res.MlpMetrics, delegate(RegressionMetrics m) { return m.Mae; }, "F2"),
                MeanStd(res.MlpMetrics, delegate(RegressionMetrics m) { return m.R2; }, "F4"));
            log.WriteLine();

            res.Bands = BandStat.Compute(test.YRaw, res.TestPredicted);
            log.WriteLine("Error by true PM2.5 band (primary model):");
            log.WriteLine(ErrorByBandText(res.Bands));
            log.WriteLine("Air-quality band classification derived from the predictions (primary model):");
            log.WriteLine(AqiCategory.Report(test.YRaw, res.TestPredicted));
            res.Confusion = AqiCategory.Confusion(test.YRaw, res.TestPredicted);
            res.BandAccuracy = res.BandAccuracies[0];
            int within = 0;
            for (int a = 0; a < AqiCategory.Names.Length; a++)
                for (int p = 0; p < AqiCategory.Names.Length; p++)
                    if (Math.Abs(a - p) <= 1) within += res.Confusion[a, p];
            res.WithinOneBand = (double)within / test.YRaw.Length;

            // 6. files for the report -------------------------------------------------------------------------------
            WritePredictions(Path.Combine(outDir, "test_predictions.csv"), records, test, res.TestPredicted);
            double[][] ex = new double[2][], ey = new double[2][];
            int n = res.PrimaryTrain.History.Count;
            for (int s = 0; s < 2; s++) { ex[s] = new double[n]; ey[s] = new double[n]; }
            for (int i = 0; i < n; i++)
            {
                ex[0][i] = ex[1][i] = res.PrimaryTrain.History[i].Epoch;
                ey[0][i] = res.PrimaryTrain.History[i].TrainMse;
                ey[1][i] = res.PrimaryTrain.History[i].ValMse;
            }
            PlotWriter.LineChart(Path.Combine(outDir, "loss_curve.png"), "Training and validation loss (scaled MSE, log axis)",
                "epoch", "MSE", new string[] { "training", "validation" }, ex, ey, true);
            PlotWriter.Scatter(Path.Combine(outDir, "actual_vs_predicted.png"),
                "Test set: actual vs predicted PM2.5", "actual PM2.5 (ug/m3)", "predicted PM2.5 (ug/m3)", test.YRaw, res.TestPredicted);

            WriteSummary(Path.Combine(outDir, "metrics_summary.txt"), best, split, inputNames, res.MeanBaseline, res.LinearBaseline,
                         res.MlpMetrics, res.BandAccuracies, bestEpochs);
            log.WriteLine("Result files written to: " + outDir);
            Stage(sink, "Done");
            return res;
        }

        // ---- helpers -------------------------------------------------------------------------------------------

        private static void Stage(ExperimentSink sink, string text) { if (sink.Stage != null) sink.Stage(text); }
        private static bool Stopped(ExperimentSink sink) { return sink.ShouldStop != null && sink.ShouldStop(); }

        private static DatasetInfo Describe(string path, List<AirQualityRecord> records, LoadReport report,
                                            DateTime first, DateTime last, DataSplit split)
        {
            DatasetInfo info = new DatasetInfo();
            info.Path = path; info.Report = report; info.First = first; info.Last = last;
            info.TrainRows = split.Train.Length; info.ValidationRows = split.Validation.Length; info.TestRows = split.Test.Length;

            int n = records.Count;
            double[] target = new double[n];
            for (int i = 0; i < n; i++) target[i] = records[i].Pm25;
            info.Columns.Add(Stat(CsvLoader.TargetColumn, target, target));
            for (int k = 0; k < CsvLoader.FeatureColumns.Count; k++)
            {
                double[] col = new double[n];
                for (int i = 0; i < n; i++) col[i] = records[i].Pollutants[k];
                info.Columns.Add(Stat(CsvLoader.FeatureColumns[k], col, target));
            }
            return info;
        }

        private static ColumnStat Stat(string name, double[] v, double[] target)
        {
            ColumnStat s = new ColumnStat();
            s.Name = name;
            double[] sorted = (double[])v.Clone();
            Array.Sort(sorted);
            s.Min = sorted[0]; s.Max = sorted[sorted.Length - 1];
            s.Median = sorted.Length % 2 == 1 ? sorted[sorted.Length / 2] : (sorted[sorted.Length / 2 - 1] + sorted[sorted.Length / 2]) / 2;
            double mean = 0, tmean = 0;
            for (int i = 0; i < v.Length; i++) { mean += v[i]; tmean += target[i]; }
            mean /= v.Length; tmean /= v.Length;
            double sxy = 0, sxx = 0, syy = 0;
            for (int i = 0; i < v.Length; i++)
            {
                sxy += (v[i] - mean) * (target[i] - tmean);
                sxx += (v[i] - mean) * (v[i] - mean);
                syy += (target[i] - tmean) * (target[i] - tmean);
            }
            s.Mean = mean;
            s.CorrelationWithTarget = (sxx > 0 && syy > 0) ? sxy / Math.Sqrt(sxx * syy) : 0;
            return s;
        }

        public delegate double Selector(RegressionMetrics m);

        public static string MeanStd(List<RegressionMetrics> list, Selector sel, string fmt)
        {
            double mean = 0;
            foreach (RegressionMetrics m in list) mean += sel(m);
            mean /= list.Count;
            double var = 0;
            foreach (RegressionMetrics m in list) var += (sel(m) - mean) * (sel(m) - mean);
            double sd = list.Count > 1 ? Math.Sqrt(var / (list.Count - 1)) : 0;
            return mean.ToString(fmt, CultureInfo.InvariantCulture) + " +/- " + sd.ToString(fmt, CultureInfo.InvariantCulture);
        }

        public static string ErrorByBandText(List<BandStat> bands)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0,-14}{1,8}{2,10}{3,10}{4,12}", "band", "rows", "MAE", "RMSE", "bias(p-a)"));
            foreach (BandStat b in bands)
                sb.AppendLine(string.Format("{0,-14}{1,8}{2,10:F2}{3,10:F2}{4,12:F2}", b.Name, b.Count, b.Mae, b.Rmse, b.Bias));
            return sb.ToString();
        }

        private static void WriteTrainingLog(string path, TrainingResult tr)
        {
            using (StreamWriter w = new StreamWriter(path))
            {
                w.WriteLine("epoch,train_mse,val_mse,val_rmse_ugm3");
                foreach (EpochRecord e in tr.History)
                    w.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0},{1:F8},{2:F8},{3:F4}", e.Epoch, e.TrainMse, e.ValMse, e.ValRmse));
            }
        }

        private static void WritePredictions(string path, List<AirQualityRecord> records, ScaledSet test, double[] pred)
        {
            using (StreamWriter w = new StreamWriter(path))
            {
                w.WriteLine("datetime,actual_pm25,predicted_pm25,error");
                for (int i = 0; i < pred.Length; i++)
                    w.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-dd HH:mm},{1:F2},{2:F2},{3:F2}",
                        records[test.RecordIndex[i]].Time, test.YRaw[i], pred[i], pred[i] - test.YRaw[i]));
            }
        }

        private static void WriteTuningPlots(string outDir, List<TuningResult> grid, double[] lrs, int[] hiddens)
        {
            foreach (bool log in new bool[] { false, true })
            {
                string[] names = new string[lrs.Length];
                double[][] xs = new double[lrs.Length][], ys = new double[lrs.Length][];
                for (int l = 0; l < lrs.Length; l++)
                {
                    names[l] = "lr " + lrs[l].ToString(CultureInfo.InvariantCulture);
                    xs[l] = new double[hiddens.Length]; ys[l] = new double[hiddens.Length];
                    for (int h = 0; h < hiddens.Length; h++)
                    {
                        xs[l][h] = hiddens[h];
                        ys[l][h] = double.NaN;
                        foreach (TuningResult r in grid)
                            if (r.Config.LogTransform == log && r.Config.LearningRate == lrs[l] && r.Config.HiddenNodes == hiddens[h] && !r.Diverged)
                                ys[l][h] = r.BestValRmse;
                    }
                }
                PlotWriter.LineChart(Path.Combine(outDir, log ? "tuning_log_transform.png" : "tuning_raw.png"),
                    "Validation RMSE vs hidden nodes (" + (log ? "log1p inputs/target" : "raw inputs/target") + ")",
                    "hidden nodes", "best validation RMSE (ug/m3)", names, xs, ys, false);
            }
        }

        private static void WriteSummary(string path, TuningResult best, DataSplit split, List<string> inputs,
                                         RegressionMetrics meanBase, RegressionMetrics olsBase, List<RegressionMetrics> mlp,
                                         List<double> bandAcc, List<int> bestEpochs)
        {
            using (StreamWriter w = new StreamWriter(path))
            {
                w.WriteLine("Chosen configuration : " + best.Config);
                w.WriteLine("Inputs (" + inputs.Count + ")         : " + string.Join(", ", inputs.ToArray()));
                w.WriteLine("Split                : train {0}, validation {1}, test {2}", split.Train.Length, split.Validation.Length, split.Test.Length);
                w.WriteLine();
                w.WriteLine("Test metrics (ug/m3)");
                w.WriteLine("  mean baseline      : " + meanBase);
                w.WriteLine("  linear regression  : " + olsBase);
                for (int i = 0; i < mlp.Count; i++)
                    w.WriteLine("  MLP seed {0}          : {1}   (best epoch {2}, band accuracy {3:P1})", i + 1, mlp[i], bestEpochs[i], bandAcc[i]);
                w.WriteLine("  MLP mean +/- sd    : RMSE {0}, MAE {1}, R2 {2}",
                    MeanStd(mlp, delegate(RegressionMetrics m) { return m.Rmse; }, "F2"),
                    MeanStd(mlp, delegate(RegressionMetrics m) { return m.Mae; }, "F2"),
                    MeanStd(mlp, delegate(RegressionMetrics m) { return m.R2; }, "F4"));
            }
        }
    }
}
