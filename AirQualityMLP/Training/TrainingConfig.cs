namespace AirQualityMLP.Training
{
    /// <summary>The hyperparameters that are tuned.</summary>
    public class TrainingConfig
    {
        public double LearningRate = 0.1;
        public int HiddenNodes = 16;
        public int MaxEpochs = 200;
        public int Patience = 30;       // stop when validation RMSE has not improved for this many epochs
        public int Seed = 1;            // weight initialisation + sample order
        public bool LogTransform = true;

        public override string ToString()
        {
            return string.Format("lr={0}, hidden={1}, log={2}, maxEpochs={3}", LearningRate, HiddenNodes, LogTransform, MaxEpochs);
        }
    }

    /// <summary>One line of the training log.</summary>
    public class EpochRecord
    {
        public int Epoch;
        public double TrainMse;   // mean squared error on the scaled target, measured while training
        public double ValMse;     // same, on validation rows
        public double ValRmse;    // validation RMSE in ug/m3 (used for early stopping)
    }

    public class TrainingResult
    {
        public System.Collections.Generic.List<EpochRecord> History = new System.Collections.Generic.List<EpochRecord>();
        public int BestEpoch;
        public double BestValRmse = double.MaxValue;
        public bool Diverged;
        public bool Cancelled;     // stopped by the user (the UI's Stop button)
        public double Seconds;
    }
}
