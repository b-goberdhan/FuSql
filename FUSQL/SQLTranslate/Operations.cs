using DataMinner.Mining.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate
{
    public class Operation
    {
        public string SQLCommand { get; set; }
        public MiningOp MiningOp  { get; set; }

        public int ClusterCount { get; set; } = -1;
        public List<string> ClusterColumns { get; set; }

        public string Description { get; set; }
        public string DescriptionTable { get; set; }
        public string GoalTable { get; set; }

        public string Title { get; set; }
        //public string Description { get; set; }

        public Operation()
        {

        }
    }
}
