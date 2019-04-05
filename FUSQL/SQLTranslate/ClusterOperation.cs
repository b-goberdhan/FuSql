using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate
{
    public class ClusterOperation : Operation
    {
        public int ClusterCount { get; set; } = -1;
        public List<string> ClusterColumns { get; set; }
        public ClusterOperation(string sqlCommand) : base(sqlCommand, DataMinner.Mining.Enums.MiningOp.Clustering) { }
    }
}
