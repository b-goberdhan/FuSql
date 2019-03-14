using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.BaseDb
{
    public interface IBaseDb<CommandResult,TRow>
    {
        void Connect();
        void Disconnect();
        CommandResult Command(string command, Action<TRow> onRowRead);
    }
}
