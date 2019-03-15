using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMinner.Mining
{
    // Output of the clustering model applied to an Iris instance
    public class ClusterPrediction
    {
        // The attributes would be used if we saved the output data
        [ColumnName("PredictedLabel")]
        public uint SelectedClusterId;
        [ColumnName("Score")]
        public float[] Distance;
    }
}
