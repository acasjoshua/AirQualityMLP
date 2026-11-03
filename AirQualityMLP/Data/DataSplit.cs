using System;

namespace AirQualityMLP.Data
{
    /// <summary>Row indices of the three subsets.</summary>
    public class DataSplit
    {
        public int[] Train;
        public int[] Validation;
        public int[] Test;

        /// <summary>
        /// Shuffles 0..count-1 with a fixed seed (Fisher-Yates) and cuts it into train / validation / test.
        /// Train is used to fit weights AND the scaling statistics, validation to choose hyperparameters and
        /// the stopping epoch, test only once at the very end.
        /// </summary>
        public static DataSplit Create(int count, double trainFraction, double validationFraction, int seed)
        {
            if (trainFraction <= 0 || validationFraction <= 0 || trainFraction + validationFraction >= 1)
                throw new ArgumentException("Fractions must be positive and leave some rows for the test set.");

            int[] index = new int[count];
            for (int i = 0; i < count; i++) index[i] = i;
            Random rnd = new Random(seed);
            for (int i = count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                int t = index[i]; index[i] = index[j]; index[j] = t;
            }

            int nTrain = (int)(count * trainFraction);
            int nVal = (int)(count * validationFraction);
            DataSplit s = new DataSplit();
            s.Train = new int[nTrain];
            s.Validation = new int[nVal];
            s.Test = new int[count - nTrain - nVal];
            Array.Copy(index, 0, s.Train, 0, nTrain);
            Array.Copy(index, nTrain, s.Validation, 0, nVal);
            Array.Copy(index, nTrain + nVal, s.Test, 0, s.Test.Length);
            return s;
        }
    }
}
