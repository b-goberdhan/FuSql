using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate
{
    public class DeleteMultiClassificationOperation : Operation
    {
        public string Name { get; set; }
        public DeleteMultiClassificationOperation() : base("", DataMinner.Mining.Enums.MiningOp.DeleteMultiClassification) { }
    }
}
