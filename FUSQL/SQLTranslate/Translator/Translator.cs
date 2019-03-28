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
            string command = "SELECT * FROM " + query.Command.Find.From + " ";
            if (query.Command.Find.Where != null && query.Command.Find.Where.Conditions.Count > 0)
            {
                command += GetWhereCondition(query.Command.Find.Where);
            }
            return command;
        }
        private static string GetWhereCondition(Where where)
        {
            string whereResult = "WHERE ";
            for (int i = 0; i < where.Conditions.Count; i++)
            {
                var condition = where.Conditions[i];
                string conditionString = "{0} {1} {2} {3} ";
                if (i > 0 && i != where.Conditions.Count)
                {
                    conditionString += "AND ";
                }
                bool hasNot = false;
                string operation = "";
                if (condition.Operation == Models.Enums.Operation.NotEqual)
                {
                    operation = "=";
                    hasNot = true;
                }
                else if(condition.Operation == Models.Enums.Operation.Equal)
                {
                    operation = "=";
                }
                else if(condition.Operation == Models.Enums.Operation.GreaterThan)
                {
                    operation = ">";
                }
                else if(condition.Operation == Models.Enums.Operation.LessThan)
                {
                    operation = "<";
                }
                conditionString = string.Format(conditionString, hasNot ? "NOT" : "", condition.ColumnName,
                    operation, condition.Value);
                whereResult += conditionString;
                
            }
            return whereResult;
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
