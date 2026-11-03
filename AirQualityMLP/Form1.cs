using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using AirQualityMLP.Data;
using AirQualityMLP.Evaluation;
using AirQualityMLP.Experiment;
using AirQualityMLP.Training;
using AirQualityMLP.UI;

namespace AirQualityMLP
{
    /// <summary>
    /// The window. The experiment itself runs on a BackgroundWorker (ExperimentRunner); this class only turns the
    /// progress messages into tables, charts and the "Try it" predictor, so the window never freezes.
    /// </summary>
    public partial class Form1 : Form
    {
        // ---- messages passed from the worker thread to the window ------------------------------------------
        private class LogMsg { public string Line; }
        private class StageMsg { public string Text; }
        private class DataMsg { public DatasetInfo Info; }
        private class TuningMsg { public int Done, Total; public TuningResult Result; }
        private class BestMsg { public TuningResult Best; }
        private class EpochMsg { public int Seed, MaxEpochs; public EpochRecord Record; }
        private class SeedMsg { public int Seed, Seeds; public TrainingResult Training; public RegressionMetrics Test; }

        /// <summary>A TextWriter that forwards every finished line to the worker's progress channel.</summary>
        private class WorkerLogWriter : TextWriter
        {
            private readonly BackgroundWorker worker;
            private readonly StringBuilder line = new StringBuilder();
            public WorkerLogWriter(BackgroundWorker worker) { this.worker = worker; }
            public override Encoding Encoding { get { return Encoding.UTF8; } }
            public override void Write(char c)
            {
                if (c == '\n') { Flush(); }
                else if (c != '\r') line.Append(c);
            }
            public override void Flush()
            {
                LogMsg m = new LogMsg();
                m.Line = line.ToString();
                line.Length = 0;
                worker.ReportProgress(-1, m);
            }
        }

        private const string ModeFull = "Full run (about 7 min)";

        private string dataPath;
        private ExperimentOptions runOptions;
        private ExperimentResult result;
        private DatasetInfo datasetInfo;
        private Stopwatch clock = new Stopwatch();
        private int currentPercent;

        // live training curve (primary network)
        private readonly List<double> liveEpoch = new List<double>();
        private readonly List<double> liveTrain = new List<double>();
        private readonly List<double> liveVal = new List<double>();

        // Try-it tab
        private NumericUpDown[] inputBoxes;
        private double? shownActual;           // measured PM2.5 of the loaded test hour (null after manual edits)
        private bool settingInputs;
        private readonly Random random = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbMode.SelectedIndex = 1;                       // quick test first: results in seconds
            dataPath = ProjectPaths.FindData();
            if (dataPath != null) txtDataPath.Text = dataPath;
            else SetStatus("Choose the air-quality CSV with Browse...");

            toolTip1.SetToolTip(btnRun, "Gradient check, data loading, grid search, final training and evaluation");
            toolTip1.SetToolTip(btnGradCheck, "Compare back-propagation with numerical derivatives (takes a moment)");
            toolTip1.SetToolTip(cmbMode, "Quick test uses a tiny grid and a few epochs; Full run reproduces the reported results");
            toolTip1.SetToolTip(btnOpenResults, "Open the folder with the CSV files and PNG charts");
            SetControlsEnabledForPrediction(false);
        }

        // ------------------------------------------------------------ toolbar

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) != DialogResult.OK) return;
            dataPath = openFileDialog1.FileName;
            txtDataPath.Text = dataPath;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (dataPath == null || !File.Exists(dataPath))
            {
                MessageBox.Show(this, "Choose the air-quality CSV file first (Browse...).", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            runOptions = new ExperimentOptions();
            runOptions.DataPath = dataPath;
            runOptions.Quick = (cmbMode.SelectedItem as string) != ModeFull;
            runOptions.OutDir = ProjectPaths.DefaultResultsDir(dataPath, runOptions.Quick);

            ResetViews();
            SetBusy(true);
            clock.Reset(); clock.Start();
            SetStatus("Starting...");
            worker.RunWorkerAsync(runOptions);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!worker.IsBusy) return;
            worker.CancelAsync();
            btnStop.Enabled = false;
            SetStatus("Stopping...");
        }

        private void btnGradCheck_Click(object sender, EventArgs e)
        {
            StringWriter sw = new StringWriter();
            bool ok = GradientCheck.Run(sw);
            txtLog.AppendText(sw.ToString().Replace("\r\n", "\n").Replace("\n", "\r\n"));
            txtLog.AppendText("\r\n");
            tabControl1.SelectedTab = tabLog;
            SetStatus(ok ? "Gradient check passed: back-propagation matches the numerical derivatives." : "Gradient check FAILED.");
        }

        private void btnOpenResults_Click(object sender, EventArgs e)
        {
            string dir = runOptions != null ? runOptions.OutDir
                       : (dataPath != null ? ProjectPaths.DefaultResultsDir(dataPath, false) : null);
            if (dir == null || !Directory.Exists(dir))
            {
                MessageBox.Show(this, "No results folder yet - run the experiment first.", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try { Process.Start(dir); }
            catch (Exception ex) { MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void SetBusy(bool busy)
        {
            btnRun.Enabled = !busy;
            btnStop.Enabled = busy;
            btnBrowse.Enabled = !busy;
            cmbMode.Enabled = !busy;
            btnGradCheck.Enabled = !busy;
            UseWaitCursor = false;
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
        }

        private void ResetViews()
        {
            txtLog.Clear();
            gridTuning.Rows.Clear();
            listEpochs.Items.Clear();
            liveEpoch.Clear(); liveTrain.Clear(); liveVal.Clear();
            lossChart.ClearChart();
            scatterChart.ClearChart();
            gridConfusion.Rows.Clear(); gridMetrics.Rows.Clear(); gridBands.Rows.Clear();
            foreach (Label l in new Label[] { cardRmseValue, cardMaeValue, cardR2Value, cardBandValue }) l.Text = "-";
            lblBest.Text = "Best configuration: (running...)";
            lblSeedStatus.Text = "";
            progressBar1.Value = 0;
            currentPercent = 0;
            result = null;
            SetControlsEnabledForPrediction(false);
        }

        // ------------------------------------------------------------ worker thread

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ExperimentOptions options = (ExperimentOptions)e.Argument;
            int seedCount = options.Quick ? 2 : 5;

            ExperimentSink sink = new ExperimentSink();
            sink.ShouldStop = delegate { return worker.CancellationPending; };
            sink.Stage = delegate(string text) { Report(currentPercent, new StageMsg { Text = text }); };
            sink.DataLoaded = delegate(DatasetInfo info) { Report(5, new DataMsg { Info = info }); };
            sink.TuningProgress = delegate(int done, int total, TuningResult r)
            {
                Report(5 + 55 * done / total, new TuningMsg { Done = done, Total = total, Result = r });
            };
            sink.BestChosen = delegate(TuningResult best) { Report(60, new BestMsg { Best = best }); };
            sink.Epoch = delegate(int seed, int maxEpochs, EpochRecord rec)
            {
                double part = (seed - 1) + Math.Min(1.0, rec.Epoch / (maxEpochs * 0.7));
                int percent = 60 + (int)(35 * part / seedCount);
                if (seed == 1 && (rec.Epoch == 1 || rec.Epoch % 5 == 0))
                    Report(percent, new EpochMsg { Seed = seed, MaxEpochs = maxEpochs, Record = rec });
                else if (rec.Epoch % 25 == 0)
                    Report(percent, null);
            };
            sink.SeedDone = delegate(int seed, int seeds, TrainingResult tr, RegressionMetrics m)
            {
                Report(60 + 35 * seed / seedCount, new SeedMsg { Seed = seed, Seeds = seeds, Training = tr, Test = m });
            };

            TextWriter log = new WorkerLogWriter(worker);
            e.Result = ExperimentRunner.Run(options, log, sink);
        }

        private void Report(int percent, object message)
        {
            currentPercent = Math.Max(0, Math.Min(100, percent));
            worker.ReportProgress(currentPercent, message);
        }

        // ------------------------------------------------------------ UI thread: progress

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            object msg = e.UserState;
            if (e.ProgressPercentage >= 0 && e.ProgressPercentage <= 100) progressBar1.Value = e.ProgressPercentage;

            if (msg is LogMsg) txtLog.AppendText(((LogMsg)msg).Line + "\r\n");
            else if (msg is StageMsg) SetStatus(((StageMsg)msg).Text + "...");
            else if (msg is DataMsg) ShowData(((DataMsg)msg).Info);
            else if (msg is TuningMsg) AddTuningRow((TuningMsg)msg);
            else if (msg is BestMsg) HighlightBest(((BestMsg)msg).Best);
            else if (msg is EpochMsg) AddEpoch((EpochMsg)msg);
            else if (msg is SeedMsg) AddSeed((SeedMsg)msg);
        }

        private void ShowData(DatasetInfo info)
        {
            datasetInfo = info;
            cardRowsValue.Text = info.Report.Loaded.ToString("N0");
            cardPeriodValue.Text = info.First.ToString("MMM yyyy") + " - " + info.Last.ToString("MMM yyyy");
            cardSplitValue.Text = info.TrainRows.ToString("N0") + " / " + info.ValidationRows.ToString("N0") + " / " + info.TestRows.ToString("N0");
            int dropped = info.Report.DataLines - info.Report.Loaded;
            cardDroppedValue.Text = dropped.ToString("N0");

            listColumns.Items.Clear();
            foreach (ColumnStat c in info.Columns)
            {
                ListViewItem item = new ListViewItem(c.Name + (c.Name == CsvLoader.TargetColumn ? "  (target)" : ""));
                item.SubItems.Add(c.Min.ToString("F2"));
                item.SubItems.Add(c.Mean.ToString("F2"));
                item.SubItems.Add(c.Median.ToString("F2"));
                item.SubItems.Add(c.Max.ToString("F2"));
                item.SubItems.Add(c.Name == CsvLoader.TargetColumn ? "-" : c.CorrelationWithTarget.ToString("F3"));
                if (c.Name == CsvLoader.TargetColumn) item.Font = new Font(listColumns.Font, FontStyle.Bold);
                listColumns.Items.Add(item);
            }
        }

        private void AddTuningRow(TuningMsg m)
        {
            TuningResult r = m.Result;
            int row = gridTuning.Rows.Add(
                r.Config.LogTransform ? "log1p" : "raw",
                r.Config.HiddenNodes.ToString(),
                r.Config.LearningRate.ToString(CultureInfo.InvariantCulture),
                r.BestEpoch.ToString(),
                r.Diverged ? "diverged" : r.BestValRmse.ToString("F2"),
                Fmt(r.ValRmseAtCheckpoint, 0), Fmt(r.ValRmseAtCheckpoint, 1), Fmt(r.ValRmseAtCheckpoint, 2), Fmt(r.ValRmseAtCheckpoint, 3),
                r.Seconds.ToString("F1"));
            gridTuning.Rows[row].Tag = r;
            gridTuning.FirstDisplayedScrollingRowIndex = Math.Max(0, row - 8);
            SetStatus(string.Format("Hyperparameter tuning: {0} of {1} configurations", m.Done, m.Total));
        }

        private static string Fmt(double[] v, int i)
        {
            return double.IsNaN(v[i]) ? "-" : v[i].ToString("F2");
        }

        /// <summary>Shades every tuning row from green (low RMSE) to white and marks the winner.</summary>
        private void HighlightBest(TuningResult best)
        {
            double min = double.MaxValue, max = double.MinValue;
            foreach (DataGridViewRow row in gridTuning.Rows)
            {
                TuningResult r = row.Tag as TuningResult;
                if (r == null || r.Diverged) continue;
                min = Math.Min(min, r.BestValRmse); max = Math.Max(max, r.BestValRmse);
            }
            foreach (DataGridViewRow row in gridTuning.Rows)
            {
                TuningResult r = row.Tag as TuningResult;
                if (r == null || r.Diverged) continue;
                double t = max > min ? (r.BestValRmse - min) / (max - min) : 0;          // 0 = best, 1 = worst
                Color c = Color.FromArgb((int)(255 - 120 * (1 - t)), 255, (int)(255 - 120 * (1 - t)));
                row.Cells[4].Style.BackColor = c;
                if (object.ReferenceEquals(r, best))
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Style.BackColor = Color.FromArgb(187, 247, 208);
                        cell.Style.Font = new Font(gridTuning.Font, FontStyle.Bold);
                    }
                    gridTuning.FirstDisplayedScrollingRowIndex = Math.Max(0, row.Index - 4);
                }
            }
            lblBest.Text = string.Format("Best configuration: learning rate {0}, {1} hidden nodes, {2} inputs  -  validation RMSE {3:F2} ug/m3",
                best.Config.LearningRate.ToString(CultureInfo.InvariantCulture), best.Config.HiddenNodes,
                best.Config.LogTransform ? "log1p" : "raw", best.BestValRmse);
        }

        private void AddEpoch(EpochMsg m)
        {
            EpochRecord r = m.Record;
            liveEpoch.Add(r.Epoch); liveTrain.Add(r.TrainMse); liveVal.Add(r.ValMse);
            lossChart.SetLine("Primary network (seed 1): loss per epoch", "epoch", "MSE (scaled)",
                new string[] { "training", "validation" },
                new double[][] { liveEpoch.ToArray(), liveEpoch.ToArray() },
                new double[][] { liveTrain.ToArray(), liveVal.ToArray() }, true);

            if (r.Epoch == 1 || r.Epoch % 10 == 0)
            {
                ListViewItem item = new ListViewItem(r.Epoch.ToString());
                item.SubItems.Add(r.TrainMse.ToString("F6"));
                item.SubItems.Add(r.ValMse.ToString("F6"));
                item.SubItems.Add(r.ValRmse.ToString("F2"));
                listEpochs.Items.Add(item);
                item.EnsureVisible();
            }
            SetStatus(string.Format("Final training (seed 1): epoch {0}, validation RMSE {1:F2} ug/m3", r.Epoch, r.ValRmse));
        }

        private void AddSeed(SeedMsg m)
        {
            string line = string.Format("Seed {0}: {1} epochs, best epoch {2}  |  test RMSE {3:F2}, MAE {4:F2}, R\u00B2 {5:F3}",
                m.Seed, m.Training.History.Count, m.Training.BestEpoch, m.Test.Rmse, m.Test.Mae, m.Test.R2);
            lblSeedStatus.Text = (lblSeedStatus.Text.Length > 0 ? lblSeedStatus.Text + "\r\n" : "") + line;
        }

        // ------------------------------------------------------------ UI thread: finished

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            clock.Stop();
            SetBusy(false);
            string took = string.Format("{0}m {1:00}s", (int)clock.Elapsed.TotalMinutes, clock.Elapsed.Seconds);

            if (e.Error != null)
            {
                SetStatus("Failed: " + e.Error.Message);
                MessageBox.Show(this, e.Error.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ExperimentResult res = (ExperimentResult)e.Result;
            if (res.Cancelled)
            {
                progressBar1.Value = 0;
                SetStatus("Stopped after " + took + ".");
                return;
            }

            result = res;
            progressBar1.Value = 100;
            ShowResults(res);
            BuildInputs(res);
            SetControlsEnabledForPrediction(true);
            UpdatePrediction();
            SetStatus("Finished in " + took + ". Results saved to " + res.OutDir);
            tabControl1.SelectedTab = tabResults;
        }

        private void ShowResults(ExperimentResult res)
        {
            RegressionMetrics m = res.MlpMetrics[0];
            cardRmseValue.Text = m.Rmse.ToString("F2");
            cardMaeValue.Text = m.Mae.ToString("F2");
            cardR2Value.Text = m.R2.ToString("F3");
            cardBandValue.Text = res.BandAccuracy.ToString("P0") + " / " + res.WithinOneBand.ToString("P0");

            scatterChart.SetScatter("Test set: actual vs predicted PM2.5", "actual PM2.5 (ug/m3)", "predicted PM2.5 (ug/m3)",
                                    res.TestActual, res.TestPredicted);

            // model comparison
            gridMetrics.Rows.Clear();
            AddMetricRow("Predict the mean", res.MeanBaseline, false);
            AddMetricRow("Linear regression", res.LinearBaseline, false);
            int mlpRow = AddMetricRow("MLP (seed 1)", m, true);
            if (res.MlpMetrics.Count > 1)
            {
                int row = gridMetrics.Rows.Add("MLP, mean of " + res.MlpMetrics.Count + " seeds",
                    Mean(res.MlpMetrics, 0).ToString("F2") + " \u00B1 " + Sd(res.MlpMetrics, 0).ToString("F2"),
                    Mean(res.MlpMetrics, 1).ToString("F2") + " \u00B1 " + Sd(res.MlpMetrics, 1).ToString("F2"),
                    Mean(res.MlpMetrics, 2).ToString("F3") + " \u00B1 " + Sd(res.MlpMetrics, 2).ToString("F3"));
            }

            // error by band
            gridBands.Rows.Clear();
            foreach (BandStat b in res.Bands)
                gridBands.Rows.Add(b.Name, b.Count.ToString(), b.Mae.ToString("F2"), b.Rmse.ToString("F2"), b.Bias.ToString("+0.00;-0.00;0.00"));

            // confusion matrix
            gridConfusion.Rows.Clear();
            for (int i = 0; i < AqiCategory.Names.Length; i++) gridConfusion.Columns[i + 1].HeaderText = AqiCategory.Names[i];
            gridConfusion.Columns[0].FillWeight = 170;
            for (int a = 0; a < AqiCategory.Names.Length; a++)
            {
                int rowSum = 0;
                for (int p = 0; p < AqiCategory.Names.Length; p++) rowSum += res.Confusion[a, p];
                int idx = gridConfusion.Rows.Add();
                DataGridViewRow row = gridConfusion.Rows[idx];
                row.Cells[0].Value = AqiCategory.Names[a];
                row.Cells[0].Style.Font = new Font(gridConfusion.Font, FontStyle.Bold);
                for (int p = 0; p < AqiCategory.Names.Length; p++)
                {
                    int count = res.Confusion[a, p];
                    DataGridViewCell cell = row.Cells[p + 1];
                    cell.Value = count == 0 ? "" : count.ToString();
                    if (a == p) cell.Style.BackColor = Color.FromArgb(187, 247, 208);
                    else if (count > 0 && rowSum > 0)
                    {
                        int shade = 255 - (int)Math.Min(90, 400.0 * count / rowSum);
                        cell.Style.BackColor = Color.FromArgb(255, shade, shade);
                    }
                }
                row.Cells[7].Value = rowSum > 0 ? ((double)res.Confusion[a, a] / rowSum).ToString("P0") : "-";
            }
        }

        private int AddMetricRow(string name, RegressionMetrics m, bool highlight)
        {
            int row = gridMetrics.Rows.Add(name, m.Rmse.ToString("F2"), m.Mae.ToString("F2"), m.R2.ToString("F3"));
            if (highlight)
                foreach (DataGridViewCell c in gridMetrics.Rows[row].Cells)
                {
                    c.Style.BackColor = Color.FromArgb(219, 234, 254);
                    c.Style.Font = new Font(gridMetrics.Font, FontStyle.Bold);
                }
            return row;
        }

        private static double Pick(RegressionMetrics m, int which) { return which == 0 ? m.Rmse : which == 1 ? m.Mae : m.R2; }
        private static double Mean(List<RegressionMetrics> l, int which)
        {
            double s = 0; foreach (RegressionMetrics m in l) s += Pick(m, which); return s / l.Count;
        }
        private static double Sd(List<RegressionMetrics> l, int which)
        {
            double mean = Mean(l, which), v = 0;
            foreach (RegressionMetrics m in l) v += (Pick(m, which) - mean) * (Pick(m, which) - mean);
            return l.Count > 1 ? Math.Sqrt(v / (l.Count - 1)) : 0;
        }

        // ------------------------------------------------------------ "Try it" tab

        private void SetControlsEnabledForPrediction(bool enabled)
        {
            flowInputs.Enabled = enabled;
            numHour.Enabled = enabled;
            btnRandomRow.Enabled = enabled;
            lblPredictHint.Visible = !enabled;
            if (!enabled)
            {
                lblPrediction.Text = "-";
                lblPredBand.Text = "";
                lblActualInfo.Text = "";
                lblPredBand.ForeColor = Color.FromArgb(100, 110, 130);
                bandScale.SetValue(null);
            }
        }

        private void BuildInputs(ExperimentResult res)
        {
            flowInputs.SuspendLayout();
            flowInputs.Controls.Clear();
            int n = CsvLoader.FeatureColumns.Count;
            inputBoxes = new NumericUpDown[n];
            settingInputs = true;
            for (int k = 0; k < n; k++)
            {
                ColumnStat stat = datasetInfo.Columns[k + 1];          // [0] is the target
                Panel row = new Panel();
                row.Size = new Size(372, 32);
                Label name = new Label();
                name.Text = stat.Name;
                name.Location = new Point(2, 7);
                name.AutoSize = true;
                name.Font = new Font("Segoe UI", 9.5f);

                NumericUpDown num = new NumericUpDown();
                num.DecimalPlaces = 2;
                num.Minimum = (decimal)stat.Min;
                num.Maximum = (decimal)stat.Max;
                num.Increment = (decimal)Math.Max(0.01, (stat.Max - stat.Min) / 100.0);
                num.Value = (decimal)Math.Min(stat.Max, Math.Max(stat.Min, stat.Median));
                num.Location = new Point(110, 4);
                num.Size = new Size(116, 25);
                num.TextAlign = HorizontalAlignment.Right;
                num.ValueChanged += new EventHandler(numInput_ValueChanged);

                Label range = new Label();
                range.Text = stat.Min.ToString("0.##") + " - " + stat.Max.ToString("0.##");
                range.Location = new Point(238, 8);
                range.AutoSize = true;
                range.ForeColor = Color.Gray;
                range.Font = new Font("Segoe UI", 8.5f);

                row.Controls.Add(name); row.Controls.Add(num); row.Controls.Add(range);
                flowInputs.Controls.Add(row);
                inputBoxes[k] = num;
            }
            numHour.Value = 12;
            settingInputs = false;
            shownActual = null;
            flowInputs.ResumeLayout();
        }

        // the right-hand panel is a Fill panel, so its children are sized from code instead of being anchored
        private void panelPredictRight_Resize(object sender, EventArgs e)
        {
            int w = Math.Max(300, panelPredictRight.ClientSize.Width - 60);
            bandScale.Width = w;
            lblActualInfo.Width = w;
            lblPredNote.Width = w;
        }

        private void numInput_ValueChanged(object sender, EventArgs e)
        {
            if (settingInputs) return;
            shownActual = null;                    // the readings were edited by hand
            UpdatePrediction();
        }

        private void btnRandomRow_Click(object sender, EventArgs e)
        {
            if (result == null) return;
            int pick = random.Next(result.TestRecordIndex.Length);
            AirQualityRecord r = result.Records[result.TestRecordIndex[pick]];
            settingInputs = true;
            for (int k = 0; k < inputBoxes.Length; k++)
            {
                decimal v = (decimal)r.Pollutants[k];
                inputBoxes[k].Value = Math.Min(inputBoxes[k].Maximum, Math.Max(inputBoxes[k].Minimum, v));
            }
            numHour.Value = r.Time.Hour;
            settingInputs = false;
            shownActual = r.Pm25;
            lblActualInfo.Tag = r.Time;
            UpdatePrediction();
        }

        private void UpdatePrediction()
        {
            if (result == null || result.Model == null || inputBoxes == null) return;

            double[] values = new double[inputBoxes.Length];
            for (int k = 0; k < values.Length; k++) values[k] = (double)inputBoxes[k].Value;
            double pm = result.Predict(values, (int)numHour.Value);

            int band = AqiCategory.FromPm25(pm);
            lblPrediction.Text = pm.ToString("F1") + " \u00B5g/m\u00B3";
            lblPredBand.Text = AqiCategory.Names[band];
            lblPredBand.ForeColor = BandScale.Colors[band];
            bandScale.SetValue(pm);

            if (shownActual.HasValue)
            {
                DateTime t = lblActualInfo.Tag is DateTime ? (DateTime)lblActualInfo.Tag : DateTime.MinValue;
                lblActualInfo.Text = string.Format("Measured at {0:d MMM yyyy, HH:mm}: {1:F1} \u00B5g/m\u00B3 ({2})\r\nEstimate is off by {3:+0.0;-0.0;0.0} \u00B5g/m\u00B3",
                    t, shownActual.Value, AqiCategory.Names[AqiCategory.FromPm25(shownActual.Value)], pm - shownActual.Value);
            }
            else lblActualInfo.Text = "Edited readings - there is no measurement to compare with.";
        }
    }
}
