using Database.BaseDb;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.SQLite
{
    public class Db : IBaseDb<object, SQLiteDataReader>
    {
        private SQLiteConnection _connection;
        public Db(string path)
        {
            _connection = new SQLiteConnection("Data Source=" + path + ";Version=3");
        }
        public object Command(string sqlCommand, Action<SQLiteDataReader> onRowRead)
        {
            var command = _connection.CreateCommand();
            command.CommandText = sqlCommand;
            var reader = command.ExecuteReader();
            while(reader.Read())
            {
                onRowRead(reader);
            }
            return null;
        }

        public void Connect()
        {
            _connection.Open();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }
    }
}
