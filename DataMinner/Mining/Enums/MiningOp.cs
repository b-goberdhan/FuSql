using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMinner.Mining.Enums
{
    public enum MiningOp
    {

        None,
        Clustering,
        BuildBinaryClassification,
        BuildMultiClassification,
        MultiClassification,
        Classify,
        DeleteMultiClassification,
        Check,
        DeleteBinaryClassification,
        ClassifyEntries
    }
}
