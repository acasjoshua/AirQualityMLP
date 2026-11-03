using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AirQualityMLP.Evaluation
{
    /// <summary>
    /// Minimal charts drawn with System.Drawing (no extra libraries). The same drawing code is used for the PNG files
    /// written by the experiment and for the live charts in the window (ChartPanel).
    /// </summary>
    public static class PlotWriter
    {
        private const int Left = 80, Right = 30, Top = 50, Bottom = 70;
        private static readonly Color[] Palette = { Color.FromArgb(31, 119, 180), Color.FromArgb(214, 39, 40),
                                                    Color.FromArgb(44, 160, 44), Color.FromArgb(255, 127, 14),
                                                    Color.FromArgb(148, 103, 189), Color.FromArgb(140, 86, 75),
                                                    Color.FromArgb(227, 119, 194) };

        // ---- PNG files -------------------------------------------------------------------------------

        public static void LineChart(string path, string title, string xLabel, string yLabel,
                                     string[] names, double[][] xs, double[][] ys, bool logY)
        {
            using (Bitmap bmp = new Bitmap(900, 560))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                DrawLineChart(g, 900, 560, title, xLabel, yLabel, names, xs, ys, logY);
                bmp.Save(path, ImageFormat.Png);
            }
        }

        public static void Scatter(string path, string title, string xLabel, string yLabel, double[] x, double[] y)
        {
            using (Bitmap bmp = new Bitmap(900, 560))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                DrawScatter(g, 900, 560, title, xLabel, yLabel, x, y);
                bmp.Save(path, ImageFormat.Png);
            }
        }

        // ---- drawing (w x h pixels, white background) ------------------------------------------------

        public static void DrawLineChart(Graphics g, int w, int h, string title, string xLabel, string yLabel,
                                         string[] names, double[][] xs, double[][] ys, bool logY)
        {
            g.Clear(Color.White);
            double xmin = double.MaxValue, xmax = double.MinValue, ymin = double.MaxValue, ymax = double.MinValue;
            for (int s = 0; s < xs.Length; s++)
                for (int i = 0; i < xs[s].Length; i++)
                {
                    if (double.IsNaN(ys[s][i]) || double.IsInfinity(ys[s][i])) continue;
                    xmin = Math.Min(xmin, xs[s][i]); xmax = Math.Max(xmax, xs[s][i]);
                    ymin = Math.Min(ymin, ys[s][i]); ymax = Math.Max(ymax, ys[s][i]);
                }
            Frame(g, w, h, title, xLabel, yLabel);
            if (xmin == double.MaxValue) { Box(g, w, h); return; }          // nothing to draw yet
            if (xmax <= xmin) xmax = xmin + 1;
            if (logY) { ymin = Math.Log10(Math.Max(ymin, 1e-12)); ymax = Math.Log10(Math.Max(ymax, 1e-12)); }
            double pad = (ymax - ymin) * 0.05 + 1e-12; ymin -= pad; ymax += pad;

            Axes(g, w, h, xmin, xmax, ymin, ymax, logY);
            for (int s = 0; s < xs.Length; s++)
            {
                using (Pen pen = new Pen(Palette[s % Palette.Length], 2f))
                {
                    bool havePrev = false; PointF prev = new PointF();
                    for (int i = 0; i < xs[s].Length; i++)
                    {
                        if (double.IsNaN(ys[s][i])) { havePrev = false; continue; }
                        double yv = logY ? Math.Log10(Math.Max(ys[s][i], 1e-12)) : ys[s][i];
                        PointF p = Map(w, h, xs[s][i], yv, xmin, xmax, ymin, ymax);
                        if (havePrev) g.DrawLine(pen, prev, p);
                        prev = p; havePrev = true;
                    }
                }
                using (Font f = new Font("Arial", 10f))
                using (Brush b = new SolidBrush(Palette[s % Palette.Length]))
                    g.DrawString(names[s], f, b, Left + 12, Top + 8 + 18 * s);
            }
            Box(g, w, h);
        }

        public static void DrawScatter(Graphics g, int w, int h, string title, string xLabel, string yLabel, double[] x, double[] y)
        {
            g.Clear(Color.White);
            double max = 0;
            for (int i = 0; i < x.Length; i++) max = Math.Max(max, Math.Max(x[i], y[i]));
            if (max <= 0) max = 1;
            max *= 1.05;
            Frame(g, w, h, title, xLabel, yLabel);
            Axes(g, w, h, 0, max, 0, max, false);
            using (Brush b = new SolidBrush(Color.FromArgb(70, Palette[0])))
                for (int i = 0; i < x.Length; i++)
                {
                    PointF p = Map(w, h, x[i], y[i], 0, max, 0, max);
                    g.FillEllipse(b, p.X - 2f, p.Y - 2f, 4f, 4f);
                }
            using (Pen pen = new Pen(Palette[1], 2f))
            {
                pen.DashStyle = DashStyle.Dash;
                g.DrawLine(pen, Map(w, h, 0, 0, 0, max, 0, max), Map(w, h, max, max, 0, max, 0, max));
            }
            using (Font f = new Font("Arial", 10f))
                g.DrawString("dashed line = perfect prediction", f, Brushes.DimGray, Left + 12, Top + 8);
            Box(g, w, h);
        }

        // ---- helpers -------------------------------------------------------------------------------------

        private static PointF Map(int w, int h, double x, double y, double xmin, double xmax, double ymin, double ymax)
        {
            float px = (float)(Left + (x - xmin) / (xmax - xmin) * (w - Left - Right));
            float py = (float)(h - Bottom - (y - ymin) / (ymax - ymin) * (h - Top - Bottom));
            return new PointF(px, py);
        }

        private static void Frame(Graphics g, int w, int h, string title, string xLabel, string yLabel)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (Font tf = new Font("Arial", 13f, FontStyle.Bold))
            using (Font lf = new Font("Arial", 11f))
            {
                g.DrawString(title, tf, Brushes.Black, Left, 12);
                SizeF xs = g.MeasureString(xLabel, lf);
                g.DrawString(xLabel, lf, Brushes.Black, Left + (w - Left - Right - xs.Width) / 2, h - 30);
                GraphicsState st = g.Save();
                g.TranslateTransform(14, Top + (h - Top - Bottom) / 2f);
                g.RotateTransform(-90);
                SizeF ys = g.MeasureString(yLabel, lf);
                g.DrawString(yLabel, lf, Brushes.Black, -ys.Width / 2, 0);
                g.Restore(st);
            }
        }

        private static void Box(Graphics g, int w, int h)
        {
            using (Pen axis = new Pen(Color.Black, 1.5f))
                g.DrawRectangle(axis, Left, Top, w - Left - Right, h - Top - Bottom);
        }

        private static void Axes(Graphics g, int w, int h, double xmin, double xmax, double ymin, double ymax, bool logY)
        {
            using (Pen grid = new Pen(Color.Gainsboro, 1f))
            using (Font f = new Font("Arial", 9f))
            {
                for (int t = 0; t <= 5; t++)
                {
                    double xv = xmin + (xmax - xmin) * t / 5.0, yv = ymin + (ymax - ymin) * t / 5.0;
                    PointF px = Map(w, h, xv, ymin, xmin, xmax, ymin, ymax), py = Map(w, h, xmin, yv, xmin, xmax, ymin, ymax);
                    g.DrawLine(grid, px.X, Top, px.X, h - Bottom);
                    g.DrawLine(grid, Left, py.Y, w - Right, py.Y);
                    g.DrawString(xv.ToString(xmax > 20 ? "F0" : "F1"), f, Brushes.Black, px.X - 10, h - Bottom + 6);
                    string yl = logY ? Math.Pow(10, yv).ToString("G3") : yv.ToString(Math.Abs(ymax) > 20 ? "F0" : "F3");
                    SizeF ysz = g.MeasureString(yl, f);
                    g.DrawString(yl, f, Brushes.Black, Left - 6 - ysz.Width, py.Y - 7);
                }
            }
        }
    }
}
