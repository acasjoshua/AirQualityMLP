namespace AirQualityMLP
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle headerStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabData = new System.Windows.Forms.TabPage();
            this.listColumns = new System.Windows.Forms.ListView();
            this.lcName = new System.Windows.Forms.ColumnHeader();
            this.lcMin = new System.Windows.Forms.ColumnHeader();
            this.lcMean = new System.Windows.Forms.ColumnHeader();
            this.lcMedian = new System.Windows.Forms.ColumnHeader();
            this.lcMax = new System.Windows.Forms.ColumnHeader();
            this.lcCorr = new System.Windows.Forms.ColumnHeader();
            this.lblDataNote = new System.Windows.Forms.Label();
            this.flowDataCards = new System.Windows.Forms.FlowLayoutPanel();
            this.cardRowsCard = new System.Windows.Forms.Panel();
            this.cardRowsCaption = new System.Windows.Forms.Label();
            this.cardRowsValue = new System.Windows.Forms.Label();
            this.cardPeriodCard = new System.Windows.Forms.Panel();
            this.cardPeriodCaption = new System.Windows.Forms.Label();
            this.cardPeriodValue = new System.Windows.Forms.Label();
            this.cardSplitCard = new System.Windows.Forms.Panel();
            this.cardSplitCaption = new System.Windows.Forms.Label();
            this.cardSplitValue = new System.Windows.Forms.Label();
            this.cardDroppedCard = new System.Windows.Forms.Panel();
            this.cardDroppedCaption = new System.Windows.Forms.Label();
            this.cardDroppedValue = new System.Windows.Forms.Label();
            this.tabTuning = new System.Windows.Forms.TabPage();
            this.gridTuning = new System.Windows.Forms.DataGridView();
            this.colScaling = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHidden = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBestEpoch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValRmse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAt50 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAt100 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAt200 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAt400 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSeconds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblBest = new System.Windows.Forms.Label();
            this.lblTuningHelp = new System.Windows.Forms.Label();
            this.tabTraining = new System.Windows.Forms.TabPage();
            this.lossChart = new AirQualityMLP.UI.ChartPanel();
            this.listEpochs = new System.Windows.Forms.ListView();
            this.lcEpoch = new System.Windows.Forms.ColumnHeader();
            this.lcTrain = new System.Windows.Forms.ColumnHeader();
            this.lcVal = new System.Windows.Forms.ColumnHeader();
            this.lcRmse = new System.Windows.Forms.ColumnHeader();
            this.lblSeedStatus = new System.Windows.Forms.Label();
            this.lblTrainingHelp = new System.Windows.Forms.Label();
            this.tabResults = new System.Windows.Forms.TabPage();
            this.tableResults = new System.Windows.Forms.TableLayoutPanel();
            this.scatterChart = new AirQualityMLP.UI.ChartPanel();
            this.grpConfusion = new System.Windows.Forms.GroupBox();
            this.gridConfusion = new System.Windows.Forms.DataGridView();
            this.colConfActual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConf0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConf1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConf2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConf3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConf4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConf5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConfRecall = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpMetrics = new System.Windows.Forms.GroupBox();
            this.gridMetrics = new System.Windows.Forms.DataGridView();
            this.colMetModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMetRmse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMetMae = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMetR2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpBands = new System.Windows.Forms.GroupBox();
            this.gridBands = new System.Windows.Forms.DataGridView();
            this.colBandName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBandRows = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBandMae = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBandRmse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBandBias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowResultCards = new System.Windows.Forms.FlowLayoutPanel();
            this.cardRmseCard = new System.Windows.Forms.Panel();
            this.cardRmseCaption = new System.Windows.Forms.Label();
            this.cardRmseValue = new System.Windows.Forms.Label();
            this.cardMaeCard = new System.Windows.Forms.Panel();
            this.cardMaeCaption = new System.Windows.Forms.Label();
            this.cardMaeValue = new System.Windows.Forms.Label();
            this.cardR2Card = new System.Windows.Forms.Panel();
            this.cardR2Caption = new System.Windows.Forms.Label();
            this.cardR2Value = new System.Windows.Forms.Label();
            this.cardBandCard = new System.Windows.Forms.Panel();
            this.cardBandCaption = new System.Windows.Forms.Label();
            this.cardBandValue = new System.Windows.Forms.Label();
            this.tabPredict = new System.Windows.Forms.TabPage();
            this.panelPredictRight = new System.Windows.Forms.Panel();
            this.lblPredCaption = new System.Windows.Forms.Label();
            this.lblPrediction = new System.Windows.Forms.Label();
            this.lblPredBand = new System.Windows.Forms.Label();
            this.bandScale = new AirQualityMLP.UI.BandScale();
            this.lblActualInfo = new System.Windows.Forms.Label();
            this.lblPredNote = new System.Windows.Forms.Label();
            this.panelPredictLeft = new System.Windows.Forms.Panel();
            this.flowInputs = new System.Windows.Forms.FlowLayoutPanel();
            this.panelHour = new System.Windows.Forms.Panel();
            this.lblHour = new System.Windows.Forms.Label();
            this.numHour = new System.Windows.Forms.NumericUpDown();
            this.btnRandomRow = new System.Windows.Forms.Button();
            this.lblPredictHint = new System.Windows.Forms.Label();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.toolPanel = new System.Windows.Forms.Panel();
            this.flowTools = new System.Windows.Forms.FlowLayoutPanel();
            this.txtDataPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblMode = new System.Windows.Forms.Label();
            this.cmbMode = new System.Windows.Forms.ComboBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnGradCheck = new System.Windows.Forms.Button();
            this.btnOpenResults = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.worker = new System.ComponentModel.BackgroundWorker();
            headerStyle.BackColor = System.Drawing.Color.FromArgb(238, 242, 248);
            headerStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            headerStyle.ForeColor = System.Drawing.Color.FromArgb(28, 40, 60);
            headerStyle.SelectionBackColor = System.Drawing.Color.FromArgb(238, 242, 248);
            headerStyle.SelectionForeColor = System.Drawing.Color.FromArgb(28, 40, 60);
            headerStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            headerStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            cellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            cellStyle.BackColor = System.Drawing.Color.White;
            cellStyle.ForeColor = System.Drawing.Color.FromArgb(28, 40, 60);
            cellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            cellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(219, 234, 254);
            cellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(28, 40, 60);
            ((System.ComponentModel.ISupportInitialize)(this.gridTuning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridConfusion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMetrics)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBands)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabData.SuspendLayout();
            this.flowDataCards.SuspendLayout();
            this.cardRowsCard.SuspendLayout();
            this.cardPeriodCard.SuspendLayout();
            this.cardSplitCard.SuspendLayout();
            this.cardDroppedCard.SuspendLayout();
            this.tabTuning.SuspendLayout();
            this.tabTraining.SuspendLayout();
            this.tabResults.SuspendLayout();
            this.tableResults.SuspendLayout();
            this.grpConfusion.SuspendLayout();
            this.grpMetrics.SuspendLayout();
            this.grpBands.SuspendLayout();
            this.flowResultCards.SuspendLayout();
            this.cardRmseCard.SuspendLayout();
            this.cardMaeCard.SuspendLayout();
            this.cardR2Card.SuspendLayout();
            this.cardBandCard.SuspendLayout();
            this.tabPredict.SuspendLayout();
            this.panelPredictRight.SuspendLayout();
            this.panelPredictLeft.SuspendLayout();
            this.flowInputs.SuspendLayout();
            this.panelHour.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.toolPanel.SuspendLayout();
            this.flowTools.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lcName
            // 
            this.lcName.Text = "Column";
            this.lcName.Width = 110;
            this.lcName.Name = "lcName";
            // 
            // lcMin
            // 
            this.lcMin.Text = "Min";
            this.lcMin.Width = 90;
            this.lcMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lcMin.Name = "lcMin";
            // 
            // lcMean
            // 
            this.lcMean.Text = "Mean";
            this.lcMean.Width = 90;
            this.lcMean.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lcMean.Name = "lcMean";
            // 
            // lcMedian
            // 
            this.lcMedian.Text = "Median";
            this.lcMedian.Width = 90;
            this.lcMedian.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lcMedian.Name = "lcMedian";
            // 
            // lcMax
            // 
            this.lcMax.Text = "Max";
            this.lcMax.Width = 90;
            this.lcMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lcMax.Name = "lcMax";
            // 
            // lcCorr
            // 
            this.lcCorr.Text = "Correlation with PM2.5";
            this.lcCorr.Width = 150;
            this.lcCorr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lcCorr.Name = "lcCorr";
            // 
            // listColumns
            // 
            this.listColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.lcName, this.lcMin, this.lcMean, this.lcMedian, this.lcMax, this.lcCorr });
            this.listColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listColumns.FullRowSelect = true;
            this.listColumns.GridLines = true;
            this.listColumns.HideSelection = false;
            this.listColumns.View = System.Windows.Forms.View.Details;
            this.listColumns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listColumns.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listColumns.Name = "listColumns";
            // 
            // lblDataNote
            // 
            this.lblDataNote.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblDataNote.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataNote.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            this.lblDataNote.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblDataNote.Size = new System.Drawing.Size(0, 56);
            this.lblDataNote.Text = "PM2.5 (first row) is the value the network learns to estimate from the other columns plus the hour of day. Inputs are min-max scaled to 0-1 using the training rows only, so nothing leaks from the test set. Run the experiment to fill this page.";
            this.lblDataNote.Name = "lblDataNote";
            // 
            // cardRowsCaption
            // 
            this.cardRowsCaption.AutoSize = true;
            this.cardRowsCaption.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardRowsCaption.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            this.cardRowsCaption.Location = new System.Drawing.Point(12, 8);
            this.cardRowsCaption.Text = "Hourly rows used";
            this.cardRowsCaption.Name = "cardRowsCaption";
            // 
            // cardRowsValue
            // 
            this.cardRowsValue.AutoSize = false;
            this.cardRowsValue.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardRowsValue.ForeColor = System.Drawing.Color.FromArgb(28, 40, 60);
            this.cardRowsValue.Location = new System.Drawing.Point(10, 30);
            this.cardRowsValue.Size = new System.Drawing.Size(246, 36);
            this.cardRowsValue.Text = "-";
            this.cardRowsValue.Name = "cardRowsValue";
            // 
            // cardRowsCard
            // 
            this.cardRowsCard.Controls.Add(this.cardRowsCaption);
            this.cardRowsCard.Controls.Add(this.cardRowsValue);
            this.cardRowsCard.BackColor = System.Drawing.Color.FromArgb(244, 246, 250);
            this.cardRowsCard.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.cardRowsCard.Size = new System.Drawing.Size(264, 78);
            this.cardRowsCard.Name = "cardRowsCard";
            // 
            // cardPeriodCaption
            // 
            this.cardPeriodCaption.AutoSize = true;
            this.cardPeriodCaption.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardPeriodCaption.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            this.cardPeriodCaption.Location = new System.Drawing.Point(12, 8);
            this.cardPeriodCaption.Text = "Period";
            this.cardPeriodCaption.Name = "cardPeriodCaption";
            // 
            // cardPeriodValue
            // 
            this.cardPeriodValue.AutoSize = false;
            this.cardPeriodValue.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardPeriodValue.ForeColor = System.Drawing.Color.FromArgb(28, 40, 60);
            this.cardPeriodValue.Location = new System.Drawing.Point(10, 30);
            this.cardPeriodValue.Size = new System.Drawing.Size(246, 36);
            this.cardPeriodValue.Text = "-";
            this.cardPeriodValue.Name = "cardPeriodValue";
            // 
            // cardPeriodCard
            // 
            this.cardPeriodCard.Controls.Add(this.cardPeriodCaption);
            this.cardPeriodCard.Controls.Add(this.cardPeriodValue);
            this.cardPeriodCard.BackColor = System.Drawing.Color.FromArgb(244, 246, 250);
            this.cardPeriodCard.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.cardPeriodCard.Size = new System.Drawing.Size(264, 78);
            this.cardPeriodCard.Name = "cardPeriodCard";
            // 
            // cardSplitCaption
            // 
            this.cardSplitCaption.AutoSize = true;
            this.cardSplitCaption.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardSplitCaption.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            this.cardSplitCaption.Location = new System.Drawing.Point(12, 8);
            this.cardSplitCaption.Text = "Train / validation / test";
            this.cardSplitCaption.Name = "cardSplitCaption";
            // 
            // cardSplitValue
            // 
            this.cardSplitValue.AutoSize = false;
            this.cardSplitValue.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardSplitValue.ForeColor = System.Drawing.Color.FromArgb(28, 40, 60);
            this.cardSplitValue.Location = new System.Drawing.Point(10, 30);
            this.cardSplitValue.Size = new System.Drawing.Size(246, 36);
            this.cardSplitValue.Text = "-";
            this.cardSplitValue.Name = "cardSplitValue";
            // 
            // cardSplitCard
            // 
            this.cardSplitCard.Controls.Add(this.cardSplitCaption);
            this.cardSplitCard.Controls.Add(this.cardSplitValue);
            this.cardSplitCard.BackColor = System.Drawing.Color.FromArgb(244, 246, 250);
            this.cardSplitCard.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.cardSplitCard.Size = new System.Drawing.Size(264, 78);
            this.cardSplitCard.Name = "cardSplitCard";
            // 
            // cardDroppedCaption
            // 
            this.cardDroppedCaption.AutoSize = true;
            this.cardDroppedCaption.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardDroppedCaption.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            this.cardDroppedCaption.Location = new System.Drawing.Point(12, 8);
            this.cardDroppedCaption.Text = "Rows dropped as invalid";
            this.cardDroppedCaption.Name = "cardDroppedCaption";
            // 
            // cardDroppedValue
            // 
            this.cardDroppedValue.AutoSize = false;
            this.cardDroppedValue.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardDroppedValue.ForeColor = System.Drawing.Color.FromArgb(28, 40, 60);
            this.cardDroppedValue.Location = new System.Drawing.Point(10, 30);
            this.cardDroppedValue.Size = new System.Drawing.Size(246, 36);
            this.cardDroppedValue.Text = "-";
            this.cardDroppedValue.Name = "cardDroppedValue";
            // 
            // cardDroppedCard
            // 
            this.cardDroppedCard.Controls.Add(this.cardDroppedCaption);
            this.cardDroppedCard.Controls.Add(this.cardDroppedValue);
            this.cardDroppedCard.BackColor = System.Drawing.Color.FromArgb(244, 246, 250);
            this.cardDroppedCard.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.cardDroppedCard.Size = new System.Drawing.Size(264, 78);
            this.cardDroppedCard.Name = "cardDroppedCard";
            // 
            // flowDataCards
            // 
            this.flowDataCards.Controls.Add(this.cardRowsCard);
            this.flowDataCards.Controls.Add(this.cardPeriodCard);
            this.flowDataCards.Controls.Add(this.cardSplitCard);
            this.flowDataCards.Controls.Add(this.cardDroppedCard);
            this.flowDataCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowDataCards.Size = new System.Drawing.Size(0, 92);
            this.flowDataCards.WrapContents = false;
            this.flowDataCards.Padding = new System.Windows.Forms.Padding(0, 0, 0, 14);
            this.flowDataCards.Name = "flowDataCards";
            // 
            // tabData
            // 
            this.tabData.Controls.Add(this.listColumns);
            this.tabData.Controls.Add(this.lblDataNote);
            this.tabData.Controls.Add(this.flowDataCards);
            this.tabData.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.tabData.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tabData.Text = "Data";
            this.tabData.Name = "tabData";
            this.tabData.UseVisualStyleBackColor = true;
            // 
            // colScaling
            // 
            this.colScaling.HeaderText = "Inputs";
            this.colScaling.Name = "colScaling";
            this.colScaling.ReadOnly = true;
            this.colScaling.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colHidden
            // 
            this.colHidden.HeaderText = "Hidden nodes";
            this.colHidden.Name = "colHidden";
            this.colHidden.ReadOnly = true;
            this.colHidden.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colLr
            // 
            this.colLr.HeaderText = "Learning rate";
            this.colLr.Name = "colLr";
            this.colLr.ReadOnly = true;
            this.colLr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colBestEpoch
            // 
            this.colBestEpoch.HeaderText = "Best epoch";
            this.colBestEpoch.Name = "colBestEpoch";
            this.colBestEpoch.ReadOnly = true;
            this.colBestEpoch.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colValRmse
            // 
            this.colValRmse.HeaderText = "Best val RMSE";
            this.colValRmse.Name = "colValRmse";
            this.colValRmse.ReadOnly = true;
            this.colValRmse.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colAt50
            // 
            this.colAt50.HeaderText = "@ 50 epochs";
            this.colAt50.Name = "colAt50";
            this.colAt50.ReadOnly = true;
            this.colAt50.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colAt100
            // 
            this.colAt100.HeaderText = "@ 100";
            this.colAt100.Name = "colAt100";
            this.colAt100.ReadOnly = true;
            this.colAt100.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colAt200
            // 
            this.colAt200.HeaderText = "@ 200";
            this.colAt200.Name = "colAt200";
            this.colAt200.ReadOnly = true;
            this.colAt200.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colAt400
            // 
            this.colAt400.HeaderText = "@ 400";
            this.colAt400.Name = "colAt400";
            this.colAt400.ReadOnly = true;
            this.colAt400.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colSeconds
            // 
            this.colSeconds.HeaderText = "Seconds";
            this.colSeconds.Name = "colSeconds";
            this.colSeconds.ReadOnly = true;
            this.colSeconds.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // gridTuning
            // 
            this.gridTuning.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colScaling, this.colHidden, this.colLr, this.colBestEpoch, this.colValRmse, this.colAt50, this.colAt100, this.colAt200, this.colAt400, this.colSeconds });
            this.gridTuning.AllowUserToAddRows = false;
            this.gridTuning.AllowUserToDeleteRows = false;
            this.gridTuning.AllowUserToResizeRows = false;
            this.gridTuning.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridTuning.BackgroundColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.gridTuning.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridTuning.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTuning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTuning.EnableHeadersVisualStyles = false;
            this.gridTuning.GridColor = System.Drawing.Color.FromArgb(228, 232, 240);
            this.gridTuning.ReadOnly = true;
            this.gridTuning.RowHeadersVisible = false;
            this.gridTuning.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTuning.MultiSelect = false;
            this.gridTuning.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.gridTuning.ColumnHeadersDefaultCellStyle = headerStyle;
            this.gridTuning.RowTemplate.Height = 26;
            this.gridTuning.DefaultCellStyle = cellStyle;
            this.gridTuning.Name = "gridTuning";
            // 
            // lblBest
            // 
            this.lblBest.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblBest.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBest.ForeColor = System.Drawing.Color.FromArgb(22, 101, 52);
            this.lblBest.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblBest.Size = new System.Drawing.Size(0, 40);
            this.lblBest.Text = "Best configuration: (run the experiment)";
            this.lblBest.Name = "lblBest";
            // 
            // lblTuningHelp
            // 
            this.lblTuningHelp.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTuningHelp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTuningHelp.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            this.lblTuningHelp.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTuningHelp.Size = new System.Drawing.Size(0, 44);
            this.lblTuningHelp.Text = "Every combination of learning rate, hidden nodes and input scaling (raw or log) is trained for a fixed epoch budget (400 in a full run) on the training rows. Validation RMSE is in ug/m3 - lower is better. The best row is highlighted.";
            this.lblTuningHelp.Name = "lblTuningHelp";
            // 
            // tabTuning
            // 
            this.tabTuning.Controls.Add(this.gridTuning);
            this.tabTuning.Controls.Add(this.lblBest);
            this.tabTuning.Controls.Add(this.lblTuningHelp);
            this.tabTuning.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.tabTuning.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.tabTuning.Text = "Tuning";
            this.tabTuning.Name = "tabTuning";
            this.tabTuning.UseVisualStyleBackColor = true;
            // 
            // lossChart
            // 
            this.lossChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lossChart.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.lossChart.Name = "lossChart";
            // 
            // lcEpoch
            // 
            this.lcEpoch.Text = "Epoch";
            this.lcEpoch.Width = 60;
            this.lcEpoch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lcEpoch.Name = "lcEpoch";
            // 
            // lcTrain
            // 
            this.lcTrain.Text = "Train MSE";
            this.lcTrain.Width = 90;
            this.lcTrain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lcTrain.Name = "lcTrain";
            // 
            // lcVal
            // 
            this.lcVal.Text = "Val MSE";
            this.lcVal.Width = 90;
            this.lcVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lcVal.Name = "lcVal";
            // 
            // lcRmse
            // 
            this.lcRmse.Text = "Val RMSE";
            this.lcRmse.Width = 100;
            this.lcRmse.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lcRmse.Name = "lcRmse";
            // 
            // listEpochs
            // 
            this.listEpochs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.lcEpoch, this.lcTrain, this.lcVal, this.lcRmse });
            this.listEpochs.Dock = System.Windows.Forms.DockStyle.Right;
            this.listEpochs.FullRowSelect = true;
            this.listEpochs.GridLines = true;
            this.listEpochs.HideSelection = false;
            this.listEpochs.View = System.Windows.Forms.View.Details;
            this.listEpochs.Size = new System.Drawing.Size(380, 0);
            this.listEpochs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listEpochs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listEpochs.Name = "listEpochs";
            // 
            // lblSeedStatus
            // 
            this.lblSeedStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSeedStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeedStatus.ForeColor = System.Drawing.Color.FromArgb(60, 70, 90);
            this.lblSeedStatus.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblSeedStatus.Size = new System.Drawing.Size(0, 92);
            this.lblSeedStatus.Text = "The primary network (seed 1) is shown live. Four more seeds are trained afterwards to measure how much the result varies.";
            this.lblSeedStatus.Name = "lblSeedStatus";
            // 
            // lblTrainingHelp
            // 
            this.lblTrainingHelp.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTrainingHelp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrainingHelp.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            this.lblTrainingHelp.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblTrainingHelp.Size = new System.Drawing.Size(0, 30);
            this.lblTrainingHelp.Text = "Loss per epoch (scaled MSE, log axis). Training loss is measured while the weights are still changing, so it can sit above the validation loss.";
            this.lblTrainingHelp.Name = "lblTrainingHelp";
            // 
            // tabTraining
            // 
            this.tabTraining.Controls.Add(this.lossChart);
            this.tabTraining.Controls.Add(this.listEpochs);
            this.tabTraining.Controls.Add(this.lblSeedStatus);
            this.tabTraining.Controls.Add(this.lblTrainingHelp);
            this.tabTraining.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.tabTraining.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.tabTraining.Text = "Training";
            this.tabTraining.Name = "tabTraining";
            this.tabTraining.UseVisualStyleBackColor = true;
            // 
            // scatterChart
            // 
            this.scatterChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scatterChart.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.scatterChart.Name = "scatterChart";
            // 
            // colConfActual
            // 
            this.colConfActual.HeaderText = "Actual";
            this.colConfActual.Name = "colConfActual";
            this.colConfActual.ReadOnly = true;
            this.colConfActual.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colConf0
            // 
            this.colConf0.HeaderText = "b0";
            this.colConf0.Name = "colConf0";
            this.colConf0.ReadOnly = true;
            this.colConf0.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colConf1
            // 
            this.colConf1.HeaderText = "b1";
            this.colConf1.Name = "colConf1";
            this.colConf1.ReadOnly = true;
            this.colConf1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colConf2
            // 
            this.colConf2.HeaderText = "b2";
            this.colConf2.Name = "colConf2";
            this.colConf2.ReadOnly = true;
            this.colConf2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colConf3
            // 
            this.colConf3.HeaderText = "b3";
            this.colConf3.Name = "colConf3";
            this.colConf3.ReadOnly = true;
            this.colConf3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colConf4
            // 
            this.colConf4.HeaderText = "b4";
            this.colConf4.Name = "colConf4";
            this.colConf4.ReadOnly = true;
            this.colConf4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colConf5
            // 
            this.colConf5.HeaderText = "b5";
            this.colConf5.Name = "colConf5";
            this.colConf5.ReadOnly = true;
            this.colConf5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colConfRecall
            // 
            this.colConfRecall.HeaderText = "recall";
            this.colConfRecall.Name = "colConfRecall";
            this.colConfRecall.ReadOnly = true;
            this.colConfRecall.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // gridConfusion
            // 
            this.gridConfusion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colConfActual, this.colConf0, this.colConf1, this.colConf2, this.colConf3, this.colConf4, this.colConf5, this.colConfRecall });
            this.gridConfusion.AllowUserToAddRows = false;
            this.gridConfusion.AllowUserToDeleteRows = false;
            this.gridConfusion.AllowUserToResizeRows = false;
            this.gridConfusion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridConfusion.BackgroundColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.gridConfusion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridConfusion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridConfusion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridConfusion.EnableHeadersVisualStyles = false;
            this.gridConfusion.GridColor = System.Drawing.Color.FromArgb(228, 232, 240);
            this.gridConfusion.ReadOnly = true;
            this.gridConfusion.RowHeadersVisible = false;
            this.gridConfusion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridConfusion.MultiSelect = false;
            this.gridConfusion.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.gridConfusion.ColumnHeadersDefaultCellStyle = headerStyle;
            this.gridConfusion.RowTemplate.Height = 26;
            this.gridConfusion.DefaultCellStyle = cellStyle;
            this.gridConfusion.Name = "gridConfusion";
            // 
            // grpConfusion
            // 
            this.grpConfusion.Controls.Add(this.gridConfusion);
            this.grpConfusion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpConfusion.Padding = new System.Windows.Forms.Padding(8, 6, 8, 8);
            this.grpConfusion.Text = "Air-quality band: actual (rows) vs predicted (columns)";
            this.grpConfusion.Name = "grpConfusion";
            // 
            // colMetModel
            // 
            this.colMetModel.HeaderText = "Model";
            this.colMetModel.Name = "colMetModel";
            this.colMetModel.ReadOnly = true;
            this.colMetModel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colMetRmse
            // 
            this.colMetRmse.HeaderText = "RMSE";
            this.colMetRmse.Name = "colMetRmse";
            this.colMetRmse.ReadOnly = true;
            this.colMetRmse.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colMetMae
            // 
            this.colMetMae.HeaderText = "MAE";
            this.colMetMae.Name = "colMetMae";
            this.colMetMae.ReadOnly = true;
            this.colMetMae.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colMetR2
            // 
            this.colMetR2.HeaderText = "R²";
            this.colMetR2.Name = "colMetR2";
            this.colMetR2.ReadOnly = true;
            this.colMetR2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // gridMetrics
            // 
            this.gridMetrics.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colMetModel, this.colMetRmse, this.colMetMae, this.colMetR2 });
            this.gridMetrics.AllowUserToAddRows = false;
            this.gridMetrics.AllowUserToDeleteRows = false;
            this.gridMetrics.AllowUserToResizeRows = false;
            this.gridMetrics.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridMetrics.BackgroundColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.gridMetrics.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridMetrics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMetrics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMetrics.EnableHeadersVisualStyles = false;
            this.gridMetrics.GridColor = System.Drawing.Color.FromArgb(228, 232, 240);
            this.gridMetrics.ReadOnly = true;
            this.gridMetrics.RowHeadersVisible = false;
            this.gridMetrics.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMetrics.MultiSelect = false;
            this.gridMetrics.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.gridMetrics.ColumnHeadersDefaultCellStyle = headerStyle;
            this.gridMetrics.RowTemplate.Height = 26;
            this.gridMetrics.DefaultCellStyle = cellStyle;
            this.gridMetrics.Name = "gridMetrics";
            // 
            // grpMetrics
            // 
            this.grpMetrics.Controls.Add(this.gridMetrics);
            this.grpMetrics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMetrics.Padding = new System.Windows.Forms.Padding(8, 6, 8, 8);
            this.grpMetrics.Text = "Test-set comparison (ug/m3)";
            this.grpMetrics.Name = "grpMetrics";
            // 
            // colBandName
            // 
            this.colBandName.HeaderText = "True band";
            this.colBandName.Name = "colBandName";
            this.colBandName.ReadOnly = true;
            this.colBandName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colBandRows
            // 
            this.colBandRows.HeaderText = "Rows";
            this.colBandRows.Name = "colBandRows";
            this.colBandRows.ReadOnly = true;
            this.colBandRows.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colBandMae
            // 
            this.colBandMae.HeaderText = "MAE";
            this.colBandMae.Name = "colBandMae";
            this.colBandMae.ReadOnly = true;
            this.colBandMae.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colBandRmse
            // 
            this.colBandRmse.HeaderText = "RMSE";
            this.colBandRmse.Name = "colBandRmse";
            this.colBandRmse.ReadOnly = true;
            this.colBandRmse.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colBandBias
            // 
            this.colBandBias.HeaderText = "Bias";
            this.colBandBias.Name = "colBandBias";
            this.colBandBias.ReadOnly = true;
            this.colBandBias.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // gridBands
            // 
            this.gridBands.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colBandName, this.colBandRows, this.colBandMae, this.colBandRmse, this.colBandBias });
            this.gridBands.AllowUserToAddRows = false;
            this.gridBands.AllowUserToDeleteRows = false;
            this.gridBands.AllowUserToResizeRows = false;
            this.gridBands.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridBands.BackgroundColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.gridBands.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridBands.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBands.EnableHeadersVisualStyles = false;
            this.gridBands.GridColor = System.Drawing.Color.FromArgb(228, 232, 240);
            this.gridBands.ReadOnly = true;
            this.gridBands.RowHeadersVisible = false;
            this.gridBands.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBands.MultiSelect = false;
            this.gridBands.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.gridBands.ColumnHeadersDefaultCellStyle = headerStyle;
            this.gridBands.RowTemplate.Height = 26;
            this.gridBands.DefaultCellStyle = cellStyle;
            this.gridBands.Name = "gridBands";
            // 
            // grpBands
            // 
            this.grpBands.Controls.Add(this.gridBands);
            this.grpBands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBands.Padding = new System.Windows.Forms.Padding(8, 6, 8, 8);
            this.grpBands.Text = "Error by true PM2.5 band";
            this.grpBands.Name = "grpBands";
            // 
            // tableResults
            // 
            this.tableResults.Controls.Add(this.scatterChart, 0, 0);
            this.tableResults.Controls.Add(this.grpConfusion, 1, 0);
            this.tableResults.Controls.Add(this.grpMetrics, 0, 1);
            this.tableResults.Controls.Add(this.grpBands, 1, 1);
            this.tableResults.ColumnCount = 2;
            this.tableResults.RowCount = 2;
            this.tableResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableResults.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableResults.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableResults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableResults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableResults.Name = "tableResults";
            // 
            // cardRmseCaption
            // 
            this.cardRmseCaption.AutoSize = true;
            this.cardRmseCaption.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardRmseCaption.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            this.cardRmseCaption.Location = new System.Drawing.Point(12, 8);
            this.cardRmseCaption.Text = "Test RMSE (ug/m3)";
            this.cardRmseCaption.Name = "cardRmseCaption";
            // 
            // cardRmseValue
            // 
            this.cardRmseValue.AutoSize = false;
            this.cardRmseValue.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardRmseValue.ForeColor = System.Drawing.Color.FromArgb(28, 40, 60);
            this.cardRmseValue.Location = new System.Drawing.Point(10, 30);
            this.cardRmseValue.Size = new System.Drawing.Size(246, 36);
            this.cardRmseValue.Text = "-";
            this.cardRmseValue.Name = "cardRmseValue";
            // 
            // cardRmseCard
            // 
            this.cardRmseCard.Controls.Add(this.cardRmseCaption);
            this.cardRmseCard.Controls.Add(this.cardRmseValue);
            this.cardRmseCard.BackColor = System.Drawing.Color.FromArgb(244, 246, 250);
            this.cardRmseCard.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.cardRmseCard.Size = new System.Drawing.Size(264, 78);
            this.cardRmseCard.Name = "cardRmseCard";
            // 
            // cardMaeCaption
            // 
            this.cardMaeCaption.AutoSize = true;
            this.cardMaeCaption.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardMaeCaption.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            this.cardMaeCaption.Location = new System.Drawing.Point(12, 8);
            this.cardMaeCaption.Text = "Test MAE (ug/m3)";
            this.cardMaeCaption.Name = "cardMaeCaption";
            // 
            // cardMaeValue
            // 
            this.cardMaeValue.AutoSize = false;
            this.cardMaeValue.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardMaeValue.ForeColor = System.Drawing.Color.FromArgb(28, 40, 60);
            this.cardMaeValue.Location = new System.Drawing.Point(10, 30);
            this.cardMaeValue.Size = new System.Drawing.Size(246, 36);
            this.cardMaeValue.Text = "-";
            this.cardMaeValue.Name = "cardMaeValue";
            // 
            // cardMaeCard
            // 
            this.cardMaeCard.Controls.Add(this.cardMaeCaption);
            this.cardMaeCard.Controls.Add(this.cardMaeValue);
            this.cardMaeCard.BackColor = System.Drawing.Color.FromArgb(244, 246, 250);
            this.cardMaeCard.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.cardMaeCard.Size = new System.Drawing.Size(264, 78);
            this.cardMaeCard.Name = "cardMaeCard";
            // 
            // cardR2Caption
            // 
            this.cardR2Caption.AutoSize = true;
            this.cardR2Caption.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardR2Caption.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            this.cardR2Caption.Location = new System.Drawing.Point(12, 8);
            this.cardR2Caption.Text = "Test R²";
            this.cardR2Caption.Name = "cardR2Caption";
            // 
            // cardR2Value
            // 
            this.cardR2Value.AutoSize = false;
            this.cardR2Value.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardR2Value.ForeColor = System.Drawing.Color.FromArgb(28, 40, 60);
            this.cardR2Value.Location = new System.Drawing.Point(10, 30);
            this.cardR2Value.Size = new System.Drawing.Size(246, 36);
            this.cardR2Value.Text = "-";
            this.cardR2Value.Name = "cardR2Value";
            // 
            // cardR2Card
            // 
            this.cardR2Card.Controls.Add(this.cardR2Caption);
            this.cardR2Card.Controls.Add(this.cardR2Value);
            this.cardR2Card.BackColor = System.Drawing.Color.FromArgb(244, 246, 250);
            this.cardR2Card.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.cardR2Card.Size = new System.Drawing.Size(264, 78);
            this.cardR2Card.Name = "cardR2Card";
            // 
            // cardBandCaption
            // 
            this.cardBandCaption.AutoSize = true;
            this.cardBandCaption.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardBandCaption.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            this.cardBandCaption.Location = new System.Drawing.Point(12, 8);
            this.cardBandCaption.Text = "Band accuracy / within one band";
            this.cardBandCaption.Name = "cardBandCaption";
            // 
            // cardBandValue
            // 
            this.cardBandValue.AutoSize = false;
            this.cardBandValue.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardBandValue.ForeColor = System.Drawing.Color.FromArgb(28, 40, 60);
            this.cardBandValue.Location = new System.Drawing.Point(10, 30);
            this.cardBandValue.Size = new System.Drawing.Size(246, 36);
            this.cardBandValue.Text = "-";
            this.cardBandValue.Name = "cardBandValue";
            // 
            // cardBandCard
            // 
            this.cardBandCard.Controls.Add(this.cardBandCaption);
            this.cardBandCard.Controls.Add(this.cardBandValue);
            this.cardBandCard.BackColor = System.Drawing.Color.FromArgb(244, 246, 250);
            this.cardBandCard.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.cardBandCard.Size = new System.Drawing.Size(264, 78);
            this.cardBandCard.Name = "cardBandCard";
            // 
            // flowResultCards
            // 
            this.flowResultCards.Controls.Add(this.cardRmseCard);
            this.flowResultCards.Controls.Add(this.cardMaeCard);
            this.flowResultCards.Controls.Add(this.cardR2Card);
            this.flowResultCards.Controls.Add(this.cardBandCard);
            this.flowResultCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowResultCards.Size = new System.Drawing.Size(0, 92);
            this.flowResultCards.WrapContents = false;
            this.flowResultCards.Padding = new System.Windows.Forms.Padding(0, 0, 0, 14);
            this.flowResultCards.Name = "flowResultCards";
            // 
            // tabResults
            // 
            this.tabResults.Controls.Add(this.tableResults);
            this.tabResults.Controls.Add(this.flowResultCards);
            this.tabResults.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.tabResults.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.tabResults.Text = "Results";
            this.tabResults.Name = "tabResults";
            this.tabResults.UseVisualStyleBackColor = true;
            // 
            // lblPredCaption
            // 
            this.lblPredCaption.AutoSize = true;
            this.lblPredCaption.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPredCaption.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            this.lblPredCaption.Location = new System.Drawing.Point(30, 18);
            this.lblPredCaption.Text = "Estimated PM2.5";
            this.lblPredCaption.Name = "lblPredCaption";
            // 
            // lblPrediction
            // 
            this.lblPrediction.AutoSize = true;
            this.lblPrediction.Font = new System.Drawing.Font("Segoe UI", 40F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrediction.ForeColor = System.Drawing.Color.FromArgb(28, 40, 60);
            this.lblPrediction.Location = new System.Drawing.Point(24, 40);
            this.lblPrediction.Text = "-";
            this.lblPrediction.Name = "lblPrediction";
            // 
            // lblPredBand
            // 
            this.lblPredBand.AutoSize = true;
            this.lblPredBand.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPredBand.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            this.lblPredBand.Location = new System.Drawing.Point(30, 118);
            this.lblPredBand.Text = "";
            this.lblPredBand.Name = "lblPredBand";
            // 
            // bandScale
            // 
            this.bandScale.Location = new System.Drawing.Point(30, 170);
            this.bandScale.Size = new System.Drawing.Size(520, 76);
            this.bandScale.Name = "bandScale";
            // 
            // lblActualInfo
            // 
            this.lblActualInfo.AutoSize = false;
            this.lblActualInfo.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActualInfo.ForeColor = System.Drawing.Color.FromArgb(60, 70, 90);
            this.lblActualInfo.Location = new System.Drawing.Point(30, 262);
            this.lblActualInfo.Size = new System.Drawing.Size(520, 48);
            this.lblActualInfo.Text = "";
            this.lblActualInfo.Name = "lblActualInfo";
            // 
            // lblPredNote
            // 
            this.lblPredNote.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPredNote.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            this.lblPredNote.Location = new System.Drawing.Point(30, 330);
            this.lblPredNote.Size = new System.Drawing.Size(520, 90);
            this.lblPredNote.Text = "Change the readings on the left and the trained network re-estimates PM2.5 instantly. \"Load a random test hour\" picks an hour the network never trained on, so you can compare the estimate with the real measurement. Bands follow the Indian CPCB PM2.5 scale (an approximation for hourly values).";
            this.lblPredNote.Name = "lblPredNote";
            // 
            // panelPredictRight
            // 
            this.panelPredictRight.Controls.Add(this.lblPredCaption);
            this.panelPredictRight.Controls.Add(this.lblPrediction);
            this.panelPredictRight.Controls.Add(this.lblPredBand);
            this.panelPredictRight.Controls.Add(this.bandScale);
            this.panelPredictRight.Controls.Add(this.lblActualInfo);
            this.panelPredictRight.Controls.Add(this.lblPredNote);
            this.panelPredictRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPredictRight.Padding = new System.Windows.Forms.Padding(28, 10, 10, 10);
            this.panelPredictRight.Name = "panelPredictRight";
            this.panelPredictRight.Resize += new System.EventHandler(this.panelPredictRight_Resize);
            // 
            // flowInputs
            // 
            this.flowInputs.AutoScroll = true;
            this.flowInputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowInputs.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowInputs.WrapContents = false;
            this.flowInputs.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.flowInputs.Name = "flowInputs";
            // 
            // lblHour
            // 
            this.lblHour.AutoSize = true;
            this.lblHour.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHour.Location = new System.Drawing.Point(2, 12);
            this.lblHour.Text = "Hour of day (0-23)";
            this.lblHour.Name = "lblHour";
            // 
            // numHour
            // 
            this.numHour.Location = new System.Drawing.Point(240, 8);
            this.numHour.Maximum = new decimal(new int[] { 23, 0, 0, 0 });
            this.numHour.Size = new System.Drawing.Size(110, 25);
            this.numHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numHour.Name = "numHour";
            this.numHour.ValueChanged += new System.EventHandler(this.numInput_ValueChanged);
            // 
            // panelHour
            // 
            this.panelHour.Controls.Add(this.lblHour);
            this.panelHour.Controls.Add(this.numHour);
            this.panelHour.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelHour.Size = new System.Drawing.Size(0, 44);
            this.panelHour.Name = "panelHour";
            // 
            // btnRandomRow
            // 
            this.btnRandomRow.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnRandomRow.Size = new System.Drawing.Size(0, 40);
            this.btnRandomRow.Text = "Load a random test hour";
            this.btnRandomRow.UseVisualStyleBackColor = true;
            this.btnRandomRow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRandomRow.Name = "btnRandomRow";
            this.btnRandomRow.Click += new System.EventHandler(this.btnRandomRow_Click);
            // 
            // lblPredictHint
            // 
            this.lblPredictHint.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPredictHint.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPredictHint.ForeColor = System.Drawing.Color.FromArgb(180, 83, 9);
            this.lblPredictHint.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblPredictHint.Size = new System.Drawing.Size(0, 44);
            this.lblPredictHint.Text = "Run the experiment first (Quick test takes seconds) - then the readings below can be edited.";
            this.lblPredictHint.Name = "lblPredictHint";
            // 
            // panelPredictLeft
            // 
            this.panelPredictLeft.Controls.Add(this.flowInputs);
            this.panelPredictLeft.Controls.Add(this.panelHour);
            this.panelPredictLeft.Controls.Add(this.btnRandomRow);
            this.panelPredictLeft.Controls.Add(this.lblPredictHint);
            this.panelPredictLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelPredictLeft.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelPredictLeft.Size = new System.Drawing.Size(400, 0);
            this.panelPredictLeft.Name = "panelPredictLeft";
            // 
            // tabPredict
            // 
            this.tabPredict.Controls.Add(this.panelPredictRight);
            this.tabPredict.Controls.Add(this.panelPredictLeft);
            this.tabPredict.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.tabPredict.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.tabPredict.Text = "Try it";
            this.tabPredict.Name = "tabPredict";
            this.tabPredict.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(20, 24, 32);
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.Color.FromArgb(210, 220, 235);
            this.txtLog.Multiline = true;
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.WordWrap = false;
            this.txtLog.Name = "txtLog";
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.txtLog);
            this.tabLog.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.tabLog.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tabLog.Text = "Log";
            this.tabLog.Name = "tabLog";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabData);
            this.tabControl1.Controls.Add(this.tabTuning);
            this.tabControl1.Controls.Add(this.tabTraining);
            this.tabControl1.Controls.Add(this.tabResults);
            this.tabControl1.Controls.Add(this.tabPredict);
            this.tabControl1.Controls.Add(this.tabLog);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Padding = new System.Drawing.Point(16, 6);
            this.tabControl1.ItemSize = new System.Drawing.Size(110, 34);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.Name = "tabControl1";
            // 
            // txtDataPath
            // 
            this.txtDataPath.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            this.txtDataPath.ReadOnly = true;
            this.txtDataPath.Size = new System.Drawing.Size(290, 25);
            this.txtDataPath.Name = "txtDataPath";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.Size = new System.Drawing.Size(84, 30);
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(0, 3, 18, 0);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = false;
            this.lblMode.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblMode.Size = new System.Drawing.Size(40, 30);
            this.lblMode.Text = "Mode";
            this.lblMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMode.Name = "lblMode";
            // 
            // cmbMode
            // 
            this.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMode.Margin = new System.Windows.Forms.Padding(0, 6, 18, 0);
            this.cmbMode.Size = new System.Drawing.Size(190, 25);
            this.cmbMode.Items.AddRange(new object[] { "Full run (about 7 min)", "Quick test (seconds)" });
            this.cmbMode.Name = "cmbMode";
            // 
            // btnRun
            // 
            this.btnRun.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRun.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRun.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.btnRun.Text = "▶  Run experiment";
            this.btnRun.Size = new System.Drawing.Size(150, 34);
            this.btnRun.UseVisualStyleBackColor = false;
            this.btnRun.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.btnRun.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRun.Name = "btnRun";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnStop
            // 
            this.btnStop.Text = "Stop";
            this.btnStop.Size = new System.Drawing.Size(70, 34);
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.Enabled = false;
            this.btnStop.Margin = new System.Windows.Forms.Padding(0, 0, 18, 0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnGradCheck
            // 
            this.btnGradCheck.Text = "Gradient check";
            this.btnGradCheck.Size = new System.Drawing.Size(112, 34);
            this.btnGradCheck.UseVisualStyleBackColor = true;
            this.btnGradCheck.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnGradCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGradCheck.Name = "btnGradCheck";
            this.btnGradCheck.Click += new System.EventHandler(this.btnGradCheck_Click);
            // 
            // btnOpenResults
            // 
            this.btnOpenResults.Text = "Results folder";
            this.btnOpenResults.Size = new System.Drawing.Size(106, 34);
            this.btnOpenResults.UseVisualStyleBackColor = true;
            this.btnOpenResults.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnOpenResults.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenResults.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnOpenResults.Name = "btnOpenResults";
            this.btnOpenResults.Click += new System.EventHandler(this.btnOpenResults_Click);
            // 
            // flowTools
            // 
            this.flowTools.Controls.Add(this.txtDataPath);
            this.flowTools.Controls.Add(this.btnBrowse);
            this.flowTools.Controls.Add(this.lblMode);
            this.flowTools.Controls.Add(this.cmbMode);
            this.flowTools.Controls.Add(this.btnRun);
            this.flowTools.Controls.Add(this.btnStop);
            this.flowTools.Controls.Add(this.btnGradCheck);
            this.flowTools.Controls.Add(this.btnOpenResults);
            this.flowTools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowTools.WrapContents = false;
            this.flowTools.Name = "flowTools";
            // 
            // toolPanel
            // 
            this.toolPanel.Controls.Add(this.flowTools);
            this.toolPanel.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.toolPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolPanel.Padding = new System.Windows.Forms.Padding(16, 12, 16, 10);
            this.toolPanel.Size = new System.Drawing.Size(0, 64);
            this.toolPanel.Name = "toolPanel";
            // 
            // lblStatus
            // 
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "Ready";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.Name = "lblStatus";
            // 
            // progressBar1
            // 
            this.progressBar1.Size = new System.Drawing.Size(240, 16);
            this.progressBar1.Name = "progressBar1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.lblStatus, this.progressBar1 });
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusStrip1.Name = "statusStrip1";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.lblTitle.Location = new System.Drawing.Point(16, 6);
            this.lblTitle.Text = "PM2.5 Estimator";
            this.lblTitle.Name = "lblTitle";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(160, 176, 204);
            this.lblSubtitle.Location = new System.Drawing.Point(19, 41);
            this.lblSubtitle.Text = "Hand-written back-propagation network  \u00B7  hourly air-quality readings, May 2019 \u2013 Jun 2020";
            this.lblSubtitle.Name = "lblSubtitle";
            // 
            // headerPanel
            // 
            this.headerPanel.Controls.Add(this.lblTitle);
            this.headerPanel.Controls.Add(this.lblSubtitle);
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(24, 36, 58);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Size = new System.Drawing.Size(0, 66);
            this.headerPanel.Name = "headerPanel";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "CSV files|*.csv|All files|*.*";
            this.openFileDialog1.Title = "Choose the air-quality CSV";
            // 
            // worker
            // 
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
            this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.worker_ProgressChanged);
            this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.ClientSize = new System.Drawing.Size(1140, 740);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolPanel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.headerPanel);
            this.MinimumSize = new System.Drawing.Size(1160, 700);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PM2.5 Estimator - back-propagation MLP";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            this.flowDataCards.ResumeLayout(false);
            this.cardRowsCard.ResumeLayout(false);
            this.cardPeriodCard.ResumeLayout(false);
            this.cardSplitCard.ResumeLayout(false);
            this.cardDroppedCard.ResumeLayout(false);
            this.tabTuning.ResumeLayout(false);
            this.tabTraining.ResumeLayout(false);
            this.tabResults.ResumeLayout(false);
            this.tableResults.ResumeLayout(false);
            this.grpConfusion.ResumeLayout(false);
            this.grpMetrics.ResumeLayout(false);
            this.grpBands.ResumeLayout(false);
            this.flowResultCards.ResumeLayout(false);
            this.cardRmseCard.ResumeLayout(false);
            this.cardMaeCard.ResumeLayout(false);
            this.cardR2Card.ResumeLayout(false);
            this.cardBandCard.ResumeLayout(false);
            this.tabPredict.ResumeLayout(false);
            this.panelPredictRight.ResumeLayout(false);
            this.panelPredictLeft.ResumeLayout(false);
            this.flowInputs.ResumeLayout(false);
            this.panelHour.ResumeLayout(false);
            this.tabLog.ResumeLayout(false);
            this.toolPanel.ResumeLayout(false);
            this.flowTools.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTuning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridConfusion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMetrics)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBands)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabData;
        private System.Windows.Forms.ListView listColumns;
        private System.Windows.Forms.ColumnHeader lcName;
        private System.Windows.Forms.ColumnHeader lcMin;
        private System.Windows.Forms.ColumnHeader lcMean;
        private System.Windows.Forms.ColumnHeader lcMedian;
        private System.Windows.Forms.ColumnHeader lcMax;
        private System.Windows.Forms.ColumnHeader lcCorr;
        private System.Windows.Forms.Label lblDataNote;
        private System.Windows.Forms.FlowLayoutPanel flowDataCards;
        private System.Windows.Forms.Panel cardRowsCard;
        private System.Windows.Forms.Label cardRowsCaption;
        private System.Windows.Forms.Label cardRowsValue;
        private System.Windows.Forms.Panel cardPeriodCard;
        private System.Windows.Forms.Label cardPeriodCaption;
        private System.Windows.Forms.Label cardPeriodValue;
        private System.Windows.Forms.Panel cardSplitCard;
        private System.Windows.Forms.Label cardSplitCaption;
        private System.Windows.Forms.Label cardSplitValue;
        private System.Windows.Forms.Panel cardDroppedCard;
        private System.Windows.Forms.Label cardDroppedCaption;
        private System.Windows.Forms.Label cardDroppedValue;
        private System.Windows.Forms.TabPage tabTuning;
        private System.Windows.Forms.DataGridView gridTuning;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScaling;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHidden;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLr;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBestEpoch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValRmse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAt50;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAt100;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAt200;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAt400;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeconds;
        private System.Windows.Forms.Label lblBest;
        private System.Windows.Forms.Label lblTuningHelp;
        private System.Windows.Forms.TabPage tabTraining;
        private AirQualityMLP.UI.ChartPanel lossChart;
        private System.Windows.Forms.ListView listEpochs;
        private System.Windows.Forms.ColumnHeader lcEpoch;
        private System.Windows.Forms.ColumnHeader lcTrain;
        private System.Windows.Forms.ColumnHeader lcVal;
        private System.Windows.Forms.ColumnHeader lcRmse;
        private System.Windows.Forms.Label lblSeedStatus;
        private System.Windows.Forms.Label lblTrainingHelp;
        private System.Windows.Forms.TabPage tabResults;
        private System.Windows.Forms.TableLayoutPanel tableResults;
        private AirQualityMLP.UI.ChartPanel scatterChart;
        private System.Windows.Forms.GroupBox grpConfusion;
        private System.Windows.Forms.DataGridView gridConfusion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfActual;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConf0;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConf1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConf2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConf3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConf4;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConf5;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfRecall;
        private System.Windows.Forms.GroupBox grpMetrics;
        private System.Windows.Forms.DataGridView gridMetrics;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMetModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMetRmse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMetMae;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMetR2;
        private System.Windows.Forms.GroupBox grpBands;
        private System.Windows.Forms.DataGridView gridBands;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBandName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBandRows;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBandMae;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBandRmse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBandBias;
        private System.Windows.Forms.FlowLayoutPanel flowResultCards;
        private System.Windows.Forms.Panel cardRmseCard;
        private System.Windows.Forms.Label cardRmseCaption;
        private System.Windows.Forms.Label cardRmseValue;
        private System.Windows.Forms.Panel cardMaeCard;
        private System.Windows.Forms.Label cardMaeCaption;
        private System.Windows.Forms.Label cardMaeValue;
        private System.Windows.Forms.Panel cardR2Card;
        private System.Windows.Forms.Label cardR2Caption;
        private System.Windows.Forms.Label cardR2Value;
        private System.Windows.Forms.Panel cardBandCard;
        private System.Windows.Forms.Label cardBandCaption;
        private System.Windows.Forms.Label cardBandValue;
        private System.Windows.Forms.TabPage tabPredict;
        private System.Windows.Forms.Panel panelPredictRight;
        private System.Windows.Forms.Label lblPredCaption;
        private System.Windows.Forms.Label lblPrediction;
        private System.Windows.Forms.Label lblPredBand;
        private AirQualityMLP.UI.BandScale bandScale;
        private System.Windows.Forms.Label lblActualInfo;
        private System.Windows.Forms.Label lblPredNote;
        private System.Windows.Forms.Panel panelPredictLeft;
        private System.Windows.Forms.FlowLayoutPanel flowInputs;
        private System.Windows.Forms.Panel panelHour;
        private System.Windows.Forms.Label lblHour;
        private System.Windows.Forms.NumericUpDown numHour;
        private System.Windows.Forms.Button btnRandomRow;
        private System.Windows.Forms.Label lblPredictHint;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Panel toolPanel;
        private System.Windows.Forms.FlowLayoutPanel flowTools;
        private System.Windows.Forms.TextBox txtDataPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.ComboBox cmbMode;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnGradCheck;
        private System.Windows.Forms.Button btnOpenResults;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripProgressBar progressBar1;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.ComponentModel.BackgroundWorker worker;
    }
}
