using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.Mining
{
    public interface IMinningOperation<TResult, TData> where TData : class
    {
        TResult Run();
        TResult LoadData(IEnumerable<TData> data);
    }
}
