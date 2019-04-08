using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate
{
    public class DeleteBinaryClassificationOperation : Operation
    {
        public string Name { get; set; }
        public DeleteBinaryClassificationOperation() : base("", DataMinner.Mining.Enums.MiningOp.DeleteBinaryClassification)
        {

        }
    }
}
