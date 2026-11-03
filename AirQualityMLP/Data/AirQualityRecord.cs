using System;

namespace AirQualityMLP.Data
{
    /// <summary>One hourly row of the CSV: when it was measured, the 11 predictor pollutants and the target (PM2.5).</summary>
    public class AirQualityRecord
    {
        public DateTime Time;
        public double[] Pollutants;   // same order as CsvLoader.FeatureColumns
        public double Pm25;           // target, ug/m3
    }
}
