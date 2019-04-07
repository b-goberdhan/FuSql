using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate.Results
{
    public class RunMultiClassifierResultView : ResultView
    {
        public string Prediction { get; set; }
        public RunMultiClassifierResultView() : base(DataMinner.Mining.Enums.MiningOp.Classify) { }
        public override string ToString()
        {
            return Prediction;
        }
    }
}
