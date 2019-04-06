using DataMinner.Mining.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate.Results
{
    public interface IResultView
    {
        string ToString();
        MiningOp MiningOp { get; }
    }
}
