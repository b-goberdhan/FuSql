using Microsoft.ML.Data;

namespace DataMinner.Mining
{
    public class BinaryClassificationData
    {
        //[LoadColumn(0)]
        public string Description;

        public string DescriptionTable;

        //[LoadColumn(1), ColumnName("Label")]
        [ColumnName("Label")]
        public string GoalTable;
    }
}
