using Database.BaseDb;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.MSSQL
{
    public class Db : BaseDb.Db
    {

        public Db(string connectionString) : base(new SqlConnection(connectionString))
        {
        }
    }
}
