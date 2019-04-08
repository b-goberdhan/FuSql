using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate
{
    public class RunBinaryClassificationEntriesOperation : Operation
    {
        public string ClassifierName { get; set; }
        public RunBinaryClassificationEntriesOperation(string sqlCommand) : base(sqlCommand, DataMinner.Mining.Enums.MiningOp.CheckEntries) { }
    }
}
