using System;
using System.Text;

namespace AirQualityMLP.Evaluation
{
    /// <summary>
    /// Indian CPCB PM2.5 bands (ug/m3). They are defined for 24-hour averages; applying them to hourly values is
    /// only an approximation, used here to turn the regression into an easy-to-read "which band" check.
    /// </summary>
    public static class AqiCategory
    {
        public static readonly string[] Names = { "Good", "Satisfactory", "Moderate", "Poor", "Very Poor", "Severe" };
        private static readonly double[] UpperBounds = { 30, 60, 90, 120, 250 };

        public static int FromPm25(double v)
        {
            for (int i = 0; i < UpperBounds.Length; i++) if (v <= UpperBounds[i]) return i;
            return UpperBounds.Length;
        }

        /// <summary>Confusion matrix text + accuracy / within-one-band accuracy / per-class precision and recall.</summary>
        public static string Report(double[] actual, double[] predicted)
        {
            int k = Names.Length;
            int[,] cm = new int[k, k];                  // [actual, predicted]
            int correct = 0, withinOne = 0;
            for (int i = 0; i < actual.Length; i++)
            {
                int a = FromPm25(actual[i]), p = FromPm25(predicted[i]);
                cm[a, p]++;
                if (a == p) correct++;
                if (Math.Abs(a - p) <= 1) withinOne++;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Confusion matrix (rows = actual band, columns = predicted band)");
            sb.Append(string.Format("{0,-14}", ""));
            foreach (string n in Names) sb.Append(string.Format("{0,13}", n));
            sb.AppendLine(string.Format("{0,9}", "recall"));
            for (int a = 0; a < k; a++)
            {
                int rowSum = 0;
                for (int p = 0; p < k; p++) rowSum += cm[a, p];
                sb.Append(string.Format("{0,-14}", Names[a]));
                for (int p = 0; p < k; p++) sb.Append(string.Format("{0,13}", cm[a, p]));
                sb.AppendLine(rowSum > 0 ? string.Format("{0,8:P0}", (double)cm[a, a] / rowSum) : string.Format("{0,9}", "-"));
            }
            sb.Append(string.Format("{0,-14}", "precision"));
            for (int p = 0; p < k; p++)
            {
                int colSum = 0;
                for (int a = 0; a < k; a++) colSum += cm[a, p];
                sb.Append(colSum > 0 ? string.Format("{0,13:P0}", (double)cm[p, p] / colSum) : string.Format("{0,13}", "-"));
            }
            sb.AppendLine();
            sb.AppendLine(string.Format("Band accuracy: {0:P1}   within one band: {1:P1}",
                (double)correct / actual.Length, (double)withinOne / actual.Length));
            return sb.ToString();
        }

        /// <summary>Counts [actual band, predicted band].</summary>
        public static int[,] Confusion(double[] actual, double[] predicted)
        {
            int[,] cm = new int[Names.Length, Names.Length];
            for (int i = 0; i < actual.Length; i++) cm[FromPm25(actual[i]), FromPm25(predicted[i])]++;
            return cm;
        }

        public static double Accuracy(double[] actual, double[] predicted)
        {
            int c = 0;
            for (int i = 0; i < actual.Length; i++) if (FromPm25(actual[i]) == FromPm25(predicted[i])) c++;
            return (double)c / actual.Length;
        }
    }
}
