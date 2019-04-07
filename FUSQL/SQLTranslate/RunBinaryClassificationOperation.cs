using FUSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate
{
    public class RunBinaryClassificationOperation : Operation
    {
        public List<Term> Terms { get; set; }
        public string BinaryClassifierName { get; set; }
        public RunBinaryClassificationOperation() : base("", DataMinner.Mining.Enums.MiningOp.Check) { }
    }
}
