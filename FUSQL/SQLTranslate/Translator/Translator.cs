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
            if (operation.MiningOp.Equals(MiningOp.Clustering))
            {
                operation.ClusterCount = TryGetClusterCount(query);
                operation.ClusterColumns = TryGetClusterColumns(query);
            }
            else if (operation.MiningOp.Equals(MiningOp.BinaryClassification))
            {
                operation.Text = TryGetBinaryClassificationText(query);
            }
            else if (operation.MiningOp.Equals(MiningOp.MultiClassification))
            {
                Console.WriteLine("MulticlassClassification!");
                operation.Title = TryGetMulticlassClassificationIssue(query);
                operation.Description = TryGetMulticlassClassificationDesc(query);
            }
            return new Translation<TRowModel>(operation);
        }

        // Determine the data mining rule/operation
        // This is used in an if-statement by the "RunX" function(s)
        private static MiningOp GetMiningOp(Query query)
        {
            // TODO: query doesn't have anything to indicate which the mining operation
            if (query.Command.Find != null)
            {
                return MiningOp.Clustering;
            }
            else if (query.Command.Check != null)
            {
                return MiningOp.BinaryClassification;
            }
            else if (query.Command.Identify != null)
            {
                return MiningOp.MultiClassification;
            }
            return MiningOp.None;
        }

        // Get and construct the SQL query
        private static string GetSqlString(Query query)
        {
            string command = "";
            if(query.Command.Find != null)
            {
                command = "SELECT * FROM " + query.Command.Find.From + " ";
                if (query.Command.Find.Where != null && query.Command.Find.Where.Conditions.Count > 0)
                {
                    command += GetWhereCondition(query.Command.Find.Where);
                }
            }
            else if(query.Command.Check != null)
            {
                command = "SELECT * FROM " + query.Command.Check.From;
            }
            else if(query.Command.Identify != null)
            {
                command = "SELECT * FROM " + query.Command.Identify.From;
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

        // Get the cluster columns
        private static List<string> TryGetClusterColumns(Query query)
        {
            var result = query.Command?.Find?.Group?.Columns;
            return result;
        }

        // Get the text
        private static string TryGetBinaryClassificationText(Query query)
        {
            var result = query.Command?.Check?.Text;
            return result;
        }

        // Get the issue text
        private static string TryGetMulticlassClassificationIssue(Query query)
        {
            var result = query.Command?.Identify?.Title;
            return result;
        }

        // Get the issue description
        private static string TryGetMulticlassClassificationDesc(Query query)
        {
            var result = query.Command?.Identify?.Description;
            return result;
        }
    }
}
