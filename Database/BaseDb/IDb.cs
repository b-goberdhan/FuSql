using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.BaseDb
{
    public interface IDb
    {
        void Connect();
        void Disconnect();
        void Command<TRowModel>(string sqlCommand, Action<TRowModel> onRowRead) where TRowModel : class, new();
    }
}
