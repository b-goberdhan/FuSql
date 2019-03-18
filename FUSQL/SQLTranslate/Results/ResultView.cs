using DataMinner.Mining.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate.Results
{
    public class ResultView<TResultType> where TResultType : class, new() 
    {
        public MiningOp MiningOp { get; private set; }
        public ResultView(MiningOp miningOp)
        {
            MiningOp = miningOp;
        }
         

    }
}
