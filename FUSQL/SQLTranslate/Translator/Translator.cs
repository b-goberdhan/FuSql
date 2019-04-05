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
            Operation operation = null;
            var miningOp = GetMiningOp(query);
            var sQLCommand = GetSqlString(query);
            
             if (miningOp == MiningOp.Clustering)
            {
                operation = new ClusterOperation(sQLCommand)
                {
                    ClusterCount = TryGetClusterCount(query),
                    ClusterColumns = TryGetClusterColumns(query)
                };
            }
            else if (miningOp == MiningOp.BinaryClassification)
            {
                
                /*
                operation = new 
                operation.Description = TryGetBinaryClassificationDescription(query);
                operation.DescriptionTable = TryGetBinaryClassificationDescriptionTable(query);
                operation.GoalTable = TryGetBinaryClassificationGoalTable(query);
                */
            }
            else if (miningOp == MiningOp.MultiClassification)
            {
                //operation.Description = TryGetMulticlassClassificationDescription(query);
                //operation.DescriptionTable = TryGetMulticlassClassificationDescriptionTable(query);
                //operation.GoalTable = TryGetMulticlassClassificationGoalTable(query);
            }
            else if (miningOp == MiningOp.BuildMultiClassification)
            {
                operation = new BuildClassificationOperation(sQLCommand)
                {
                    Goal = query.Command.Create.Mapping.GoalColumn,
                    InputColumns = query.Command.Create.Mapping.InputColumns
                };

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
            else if (query.Command.Create?.Mapping != null)
            {
                return MiningOp.BuildMultiClassification;
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
            else if(query.Command.Create?.Mapping != null)
            {
                var columns = query.Command.Create.Mapping.GoalColumn;
                foreach (var col in query.Command.Create.Mapping.InputColumns)
                {
                    columns += ", " + col;
                }
                command = "SELECT " + columns + " FROM " + query.Command.Create.Mapping.From;
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
        private static string TryGetBinaryClassificationDescription(Query query)
        {
            var result = query.Command?.Check?.Description;
            return result;
        }

        // Get the text
        private static string TryGetBinaryClassificationDescriptionTable(Query query)
        {
            var result = query.Command?.Check?.DescriptionTable;
            return result;
        }

        // Get the text
        private static string TryGetBinaryClassificationGoalTable(Query query)
        {
            var result = query.Command?.Check?.GoalTable;
            return result;
        }

        // Get the issue text
        private static string TryGetMulticlassClassificationDescription(Query query)
        {
            var result = query.Command?.Identify?.Description;
            return result;
        }

        // Get the issue description
        private static string TryGetMulticlassClassificationDescriptionTable(Query query)
        {
            var result = query.Command?.Identify?.DescriptionTable;
            return result;
        }

        private static string TryGetMulticlassClassificationGoalTable(Query query)
        {
            var result = query.Command?.Identify?.GoalTable;
            return result;
        }
    }
}
