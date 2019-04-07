using Microsoft.ML.Data;

namespace DataMinner.Mining
{
    public class BinaryClassificationPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public float Probability { get; set; } = -1;

        public float Score { get; set; }
    }
}
