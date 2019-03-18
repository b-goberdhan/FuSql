using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.BaseDb
{
    public abstract class Db : IBaseDb
    {
        protected readonly DbConnection _connection;
        public Db(DbConnection connection)
        {
            _connection = connection;
        }
        public void Connect()
        {
            _connection.Open();
        }

        public void Disconnect()
        {
            _connection.Close();
        }
        public virtual void Command<TRowModel>(string sqlCommand, Action<TRowModel> onRowRead) where TRowModel : class, new()
        {
            var command = _connection.CreateCommand();
            command.CommandText = sqlCommand;
            var reader = command.ExecuteReader();
            List<string> columnNames = new List<string>(reader.FieldCount);
            List<Type> columnTypes = new List<Type>(reader.FieldCount);
            for (int i = 0; i < reader.FieldCount; i++)
            {
                columnNames.Add(reader.GetName(i));
                columnTypes.Add(reader.GetFieldType(i));
            }
            while (reader.Read())
            {

                List<object> columnValues = new List<object>(reader.FieldCount);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    columnValues.Add(reader[columnNames[i]]);
                }
                var rowModel = TryParseToObject<TRowModel>(columnNames, columnTypes, columnValues);
                onRowRead(rowModel);
            }
        }

        private TRowModel TryParseToObject<TRowModel>(List<string> columnNames, List<Type> columnTypes, List<object> columnValues)
        {
            Type type = typeof(TRowModel);
            TRowModel rowModel = (TRowModel)Activator.CreateInstance(type);
            for (int i = 0; i < columnNames.Count; i++)
            {
                var property = rowModel.GetType().GetProperty(columnNames[i]);
                if (property != null && property.PropertyType == columnTypes[i])
                {
                    property.SetValue(rowModel, Convert.ChangeType(columnValues[i], columnTypes[i]));
                }
                else if (property != null && property.PropertyType != columnTypes[i])
                {
                    property.SetValue(rowModel, Convert.ChangeType(columnValues[i], property.PropertyType));
                }
            }
            return rowModel;
        }
    }
}
