using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMinner.Mining.Enums;
using FUSQL.Models;

namespace FUSQL.SQLTranslate.Translator
{
    public class Translator
    {
        public static Translation<TRowModel> TranslateQuery<TRowModel>(Query query) where TRowModel : class, new()
        {
            Operation operation = new Operation();
            operation.MiningOp = GetMiningOp(query);
            operation.SQLCommand = GetSqlString(query);
            operation.ClusterCount = TryGetClusterCount(query);
            operation.ClusterColumns = TryGetClusterColumns(query);
            return new Translation<TRowModel>(operation);
        }
        private static MiningOp GetMiningOp(Query query)
        {
            if (query.Command.Find != null)
            {
                return MiningOp.Clustering;
            }
            return MiningOp.None;
        }
        private static string GetSqlString(Query query)
        {
            string command = "SELECT * FROM " + query.Command.Find.From;
            return command;
        }
        private static int TryGetClusterCount(Query query)
        {
            var result = query.Command?.Find?.Group?.Count;
            if (result == null)
            {
                return -1;
            }
            else
            {
                return (int)result;
            }
            
        }
        private static List<string> TryGetClusterColumns(Query query)
        {
            var result = query.Command?.Find?.Group?.Columns;
            return result;
        }
    }
}
