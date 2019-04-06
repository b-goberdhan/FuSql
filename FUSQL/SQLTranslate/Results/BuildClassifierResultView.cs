using DataMinner.Mining.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate.Results
{
    public class BuildClassifierResultView : ResultView
    {
        public string Name { get; set; }
        public BuildClassifierResultView() : base(MiningOp.BuildMultiClassification)
        {

        }
        public override string ToString()
        {
            return Name;
        }
    }
}
