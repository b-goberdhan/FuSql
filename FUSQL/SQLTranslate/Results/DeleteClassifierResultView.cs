using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate.Results
{
    public class DeleteMultiClassifierResultView : ResultView
    {
        public string ClassifierName { get; set; }
        public DeleteMultiClassifierResultView() : base(DataMinner.Mining.Enums.MiningOp.DeleteMultiClassification) { }
        public override string ToString()
        {
            return "Deleted: " + ClassifierName;
        }
    }
}
