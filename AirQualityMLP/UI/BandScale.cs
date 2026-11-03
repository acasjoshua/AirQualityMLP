using System;
using System.Drawing;
using System.Windows.Forms;
using AirQualityMLP.Evaluation;

namespace AirQualityMLP.UI
{
    /// <summary>The six PM2.5 air-quality bands as a coloured bar with a marker at the estimated value.</summary>
    public class BandScale : Control
    {
        public static readonly Color[] Colors =
        {
            Color.FromArgb(34, 160, 80),   // Good
            Color.FromArgb(140, 198, 63),  // Satisfactory
            Color.FromArgb(245, 190, 20),  // Moderate
            Color.FromArgb(240, 120, 30),  // Poor
            Color.FromArgb(220, 50, 47),   // Very Poor
            Color.FromArgb(150, 20, 40)    // Severe
        };

        private static readonly double[] Bounds = { 0, 30, 60, 90, 120, 250, 500 };

        private double? value;

        public BandScale()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = Color.White;
        }

        public void SetValue(double? v)
        {
            value = v;
            Invalidate();
        }

        private static double Position(double v)         // 0..1 along the bar, every band the same width
        {
            for (int i = 0; i < Bounds.Length - 1; i++)
                if (v <= Bounds[i + 1] || i == Bounds.Length - 2)
                {
                    double frac = (v - Bounds[i]) / (Bounds[i + 1] - Bounds[i]);
                    frac = Math.Max(0, Math.Min(1, frac));
                    return (i + frac) / (Bounds.Length - 1);
                }
            return 1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(BackColor);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int n = Colors.Length;
            float left = 2, width = Width - 6, barTop = 26, barH = 20;
            float segW = width / n;

            using (Font small = new Font("Segoe UI", 8f))
            {
                for (int i = 0; i < n; i++)
                {
                    using (Brush b = new SolidBrush(Colors[i]))
                        g.FillRectangle(b, left + i * segW, barTop, segW - 2, barH);
                    string label = AqiCategory.Names[i];
                    SizeF sz = g.MeasureString(label, small);
                    g.DrawString(label, small, Brushes.DimGray, left + i * segW + (segW - 2 - sz.Width) / 2, barTop + barH + 4);
                    string range = i < n - 1 ? "<= " + Bounds[i + 1] : "> " + Bounds[i];
                    SizeF rz = g.MeasureString(range, small);
                    g.DrawString(range, small, Brushes.Gray, left + i * segW + (segW - 2 - rz.Width) / 2, barTop + barH + 18);
                }
            }

            if (value.HasValue)
            {
                float x = left + (float)(Position(value.Value) * width);
                using (Brush b = new SolidBrush(Color.FromArgb(28, 40, 60)))
                {
                    g.FillPolygon(b, new PointF[] { new PointF(x - 7, 4), new PointF(x + 7, 4), new PointF(x, 20) });
                    g.FillRectangle(b, x - 1.5f, 20, 3, barH + 8);
                }
            }
        }
    }
}
