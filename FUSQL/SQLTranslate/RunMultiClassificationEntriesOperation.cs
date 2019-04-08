using DataMinner.Mining.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate
{
    public class RunMultiClassificationEntriesOperation : Operation
    {
        public string ClassifierName { get; set; }
        public RunMultiClassificationEntriesOperation(string sqlCommand) : base(sqlCommand, MiningOp.ClassifyEntries) { }
    }
}
