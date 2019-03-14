using Database.BaseDb;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.SQLite
{
    public class SqliteDb : BaseDb.Db
    {
        public SqliteDb(string path) : base(new SQLiteConnection("Data Source=" + path + ";Version=3"))
        {
        }       
    }
}
