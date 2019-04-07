using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate
{
    public class BuildBinaryClassificationOperation : Operation
    {
        public List<string> InputColumns { get; set; }
        public string Goal { get; set; }
        public string Name { get; set; }
        public BuildBinaryClassificationOperation(string sqlCommand) : base(sqlCommand, DataMinner.Mining.Enums.MiningOp.BuildBinaryClassification)
        {

        }
    }
}
