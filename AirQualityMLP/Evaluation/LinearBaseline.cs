using System;
using AirQualityMLP.Data;

namespace AirQualityMLP.Evaluation
{
    /// <summary>
    /// Reference only (NOT the neural network): ordinary least squares on the same scaled inputs, solved with the
    /// normal equations. It tells us whether the MLP is actually adding value over a straight-line model.
    /// </summary>
    public class LinearBaseline
    {
        private double[] w;     // last element = intercept
        private readonly Preprocessor pre;

        public LinearBaseline(Preprocessor pre) { this.pre = pre; }

        public void Fit(ScaledSet train)
        {
            int n = train.X.Length, d = train.X[0].Length + 1;
            double[,] a = new double[d, d + 1];                  // augmented matrix [X'X | X'y]
            for (int r = 0; r < n; r++)
            {
                for (int i = 0; i < d; i++)
                {
                    double xi = i < d - 1 ? train.X[r][i] : 1.0;
                    for (int j = 0; j < d; j++) a[i, j] += xi * (j < d - 1 ? train.X[r][j] : 1.0);
                    a[i, d] += xi * train.YScaled[r];
                }
            }
            for (int i = 0; i < d; i++) a[i, i] += 1e-9;           // tiny ridge for numerical safety

            for (int col = 0; col < d; col++)                      // Gauss-Jordan with partial pivoting
            {
                int pivot = col;
                for (int r = col + 1; r < d; r++) if (Math.Abs(a[r, col]) > Math.Abs(a[pivot, col])) pivot = r;
                if (pivot != col)
                    for (int j = 0; j <= d; j++) { double t = a[col, j]; a[col, j] = a[pivot, j]; a[pivot, j] = t; }
                double p = a[col, col];
                for (int j = col; j <= d; j++) a[col, j] /= p;
                for (int r = 0; r < d; r++)
                {
                    if (r == col) continue;
                    double f = a[r, col];
                    if (f == 0) continue;
                    for (int j = col; j <= d; j++) a[r, j] -= f * a[col, j];
                }
            }
            w = new double[d];
            for (int i = 0; i < d; i++) w[i] = a[i, d];
        }

        public double[] Predict(ScaledSet set)
        {
            double[] result = new double[set.X.Length];
            for (int r = 0; r < result.Length; r++)
            {
                double s = w[w.Length - 1];
                for (int i = 0; i < set.X[r].Length; i++) s += w[i] * set.X[r][i];
                result[r] = pre.InverseTarget(s);
            }
            return result;
        }
    }
}
