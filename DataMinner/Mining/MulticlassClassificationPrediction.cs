using Microsoft.ML.Data;

namespace DataMinner.Mining
{
    public class MulticlassClassificationPrediction
    {
        [ColumnName("PredictedLabel")]
        public string GoalTable;

        //public float Probability { get; set; }
    }
}
