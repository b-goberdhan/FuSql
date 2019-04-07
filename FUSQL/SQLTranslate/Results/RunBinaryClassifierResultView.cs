using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate.Results
{
    public class RunBinaryClassifierResultView : ResultView
    {
        public bool Prediction { get; set; }
        public float Probability { get; set; }
        public RunBinaryClassifierResultView() : base(DataMinner.Mining.Enums.MiningOp.Check) { }

        public override string ToString()
        {
            return (Prediction ? "Positive" : "Negative") + " with " + Probability + " probability";
        }
    }
}
