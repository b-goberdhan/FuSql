using Microsoft.ML.Data;

namespace DataMinner.Mining
{
    public class BinaryClassificationData
    {
        [LoadColumn(0)]
        public string SentimentText;

        [LoadColumn(1), ColumnName("Label")]
        public bool Sentiment;
    }
}
