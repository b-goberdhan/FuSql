using DataMinner.Mining.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate
{
    public abstract class Operation
    {
        public string SQLCommand { get; private set; }
        public MiningOp MiningOp  { get; private set; }

        //public string Description { get; set; }

        public Operation(string sqlCommand, MiningOp miningOp)
        {
            SQLCommand = sqlCommand;
            MiningOp = miningOp;
        }
    }
}
