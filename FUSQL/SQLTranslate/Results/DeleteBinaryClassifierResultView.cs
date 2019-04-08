using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate.Results
{
    public class DeleteBinaryClassifierResultView : ResultView
    {
        public string Name { get; set; }
        public DeleteBinaryClassifierResultView() : base(DataMinner.Mining.Enums.MiningOp.DeleteBinaryClassification) { }
        public override string ToString()
        {
            return "Deleted: " + Name;
        }
    }
}
