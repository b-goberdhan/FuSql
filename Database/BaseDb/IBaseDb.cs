using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.BaseDb
{
    public interface IBaseDb
    {
        void Connect();
        void Disconnect();
    }
}
