using DataMinner.Mining.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate.Results
{
    public class BuildMultiClassifierResultView : ResultView
    {
        public string Name { get; set; }
        public BuildMultiClassifierResultView() : base(MiningOp.BuildMultiClassification)
        {

        }
        public override string ToString()
        {
            return Name;
        }
    }
}
