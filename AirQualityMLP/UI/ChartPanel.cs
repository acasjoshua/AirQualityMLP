using System;
using System.Drawing;
using System.Windows.Forms;
using AirQualityMLP.Evaluation;

namespace AirQualityMLP.UI
{
    /// <summary>A control that shows a line chart or an actual-vs-predicted scatter plot (same drawing code as the PNG files).</summary>
    public class ChartPanel : Control
    {
        private enum Kind { None, Line, Scatter }

        private Kind kind = Kind.None;
        private string title, xLabel, yLabel;
        private string[] names;
        private double[][] xs, ys;
        private bool logY;
        private double[] sx, sy;

        public string EmptyText { get; set; }

        public ChartPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = Color.White;
            EmptyText = "Run the experiment to see this chart.";
        }

        public void SetLine(string title, string xLabel, string yLabel, string[] names, double[][] xs, double[][] ys, bool logY)
        {
            kind = Kind.Line;
            this.title = title; this.xLabel = xLabel; this.yLabel = yLabel;
            this.names = names; this.xs = xs; this.ys = ys; this.logY = logY;
            Invalidate();
        }

        public void SetScatter(string title, string xLabel, string yLabel, double[] x, double[] y)
        {
            kind = Kind.Scatter;
            this.title = title; this.xLabel = xLabel; this.yLabel = yLabel;
            sx = x; sy = y;
            Invalidate();
        }

        public void ClearChart()
        {
            kind = Kind.None;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int w = ClientSize.Width, h = ClientSize.Height;
            if (kind == Kind.None || w < 260 || h < 200)
            {
                g.Clear(BackColor);
                if (kind == Kind.None && w > 40)
                    using (Font f = new Font("Segoe UI", 10f))
                    {
                        SizeF size = g.MeasureString(EmptyText, f);
                        g.DrawString(EmptyText, f, Brushes.Gray, (w - size.Width) / 2, (h - size.Height) / 2);
                    }
                return;
            }
            if (kind == Kind.Line) PlotWriter.DrawLineChart(g, w, h, title, xLabel, yLabel, names, xs, ys, logY);
            else PlotWriter.DrawScatter(g, w, h, title, xLabel, yLabel, sx, sy);
        }
    }
}
