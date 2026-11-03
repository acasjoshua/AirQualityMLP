using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace AirQualityMLP.Data
{
    /// <summary>What happened while reading the file (printed so data problems are never silent).</summary>
    public class LoadReport
    {
        public int DataLines;          // non-empty lines after the header
        public int Loaded;             // rows kept
        public int DroppedMalformed;   // wrong number of columns
        public int DroppedBadDate;
        public int DroppedMissing;     // empty / NA / not a number
        public int DroppedNegative;    // a concentration below zero is a sensor fault
    }

    /// <summary>
    /// Reads the air-quality CSV (Datetime,PM2.5,PM10,NO2,...). Columns are found by name, numbers are
    /// parsed with the invariant culture, and any row that cannot be trusted is dropped and counted.
    /// </summary>
    public static class CsvLoader
    {
        public const string TargetColumn = "PM2.5";
        public const string TimeColumn = "Datetime";

        private static readonly string[] DateFormats =
            { "d/M/yyyy H:mm", "dd/MM/yyyy HH:mm", "d/M/yyyy H:mm:ss", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm" };

        private static readonly string[] MissingTokens = { "", "NA", "N/A", "NaN", "null", "NULL", "-", "--" };

        /// <summary>Names of the predictor columns, in file order (filled by Load).</summary>
        public static List<string> FeatureColumns = new List<string>();

        public static List<AirQualityRecord> Load(string path, out LoadReport report)
        {
            report = new LoadReport();
            List<AirQualityRecord> records = new List<AirQualityRecord>();

            using (StreamReader reader = new StreamReader(path))
            {
                string headerLine = reader.ReadLine();
                if (headerLine == null) throw new InvalidDataException("The CSV file is empty.");
                string[] header = headerLine.Split(',');
                for (int i = 0; i < header.Length; i++) header[i] = header[i].Trim();

                int timeCol = Array.IndexOf(header, TimeColumn);
                int targetCol = Array.IndexOf(header, TargetColumn);
                if (timeCol < 0 || targetCol < 0)
                    throw new InvalidDataException("The CSV needs '" + TimeColumn + "' and '" + TargetColumn + "' columns.");

                List<int> featureCols = new List<int>();
                FeatureColumns = new List<string>();
                for (int i = 0; i < header.Length; i++)
                {
                    if (i == timeCol || i == targetCol) continue;
                    featureCols.Add(i);
                    FeatureColumns.Add(header[i]);
                }

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Trim().Length == 0) continue;
                    report.DataLines++;

                    string[] f = line.Split(',');
                    if (f.Length != header.Length) { report.DroppedMalformed++; continue; }

                    DateTime time;
                    if (!DateTime.TryParseExact(f[timeCol].Trim(), DateFormats, CultureInfo.InvariantCulture,
                                                DateTimeStyles.None, out time))
                    { report.DroppedBadDate++; continue; }

                    double target;
                    if (!TryParseNumber(f[targetCol], out target)) { report.DroppedMissing++; continue; }

                    double[] values = new double[featureCols.Count];
                    bool ok = true;
                    for (int k = 0; k < featureCols.Count && ok; k++)
                        ok = TryParseNumber(f[featureCols[k]], out values[k]);
                    if (!ok) { report.DroppedMissing++; continue; }

                    bool negative = target < 0;
                    for (int k = 0; k < values.Length; k++) if (values[k] < 0) negative = true;
                    if (negative) { report.DroppedNegative++; continue; }

                    AirQualityRecord r = new AirQualityRecord();
                    r.Time = time;
                    r.Pollutants = values;
                    r.Pm25 = target;
                    records.Add(r);
                    report.Loaded++;
                }
            }
            return records;
        }

        private static bool TryParseNumber(string text, out double value)
        {
            text = text.Trim();
            value = 0;
            if (Array.IndexOf(MissingTokens, text) >= 0) return false;
            if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value)) return false;
            return !double.IsNaN(value) && !double.IsInfinity(value);
        }
    }
}
