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
            else if (miningOp == MiningOp.BuildBinaryClassification)
            {
                operation = new BuildBinaryClassificationOperation(sQLCommand)
                {
                    Name = query.Command.Create.BinaryClassification.Name,
                    Goal = query.Command.Create.BinaryClassification.GoalColumn,
                    InputColumns = query.Command.Create.BinaryClassification.InputColumns
                };
            }
            else if (miningOp == MiningOp.Classify)
            {
                operation = new RunClassificationOperation(miningOp)
                {
                    ClassifierName = query.Command.Classify.ClassifierName,
                    Terms = query.Command.Classify.Terms,                   
                };
            }
            else if (miningOp == MiningOp.ClassifyEntries)
            {
                operation = new RunMultiClassificationEntriesOperation(sQLCommand)
                {
                    ClassifierName = query.Command.Classify.ClassifierName
                };
            }
            else if (miningOp == MiningOp.Check)
            {
                operation = new RunBinaryClassificationOperation()
                {
                    BinaryClassifierName = query.Command.Check.BinaryClassifierName,
                    Terms = query.Command.Check.Terms
                };
            }
            else if (miningOp == MiningOp.DeleteBinaryClassification)
            {
                operation = new DeleteBinaryClassificationOperation()
                {
                    Name = query.Command.Delete.DeleteBinaryClassificationName
                };
            }
            else if (miningOp == MiningOp.DeleteMultiClassification)
            {
                operation = new DeleteMultiClassificationOperation()
                {
                    Name = query.Command.Delete.DeleteMultiClassifictionName
                };
            }
            else if (miningOp == MiningOp.BuildMultiClassification)
            {
                operation = new BuildMultiClassificationOperation(sQLCommand)
                {
                    Name = query.Command.Create.MultiClassification.Name,
                    Goal = query.Command.Create.MultiClassification.GoalColumn,
                    InputColumns = query.Command.Create.MultiClassification.InputColumns
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
            else if (query.Command.Identify != null)
            {
                return MiningOp.MultiClassification;
            }
            else if (query.Command.Delete?.DeleteMultiClassifictionName != null)
            {
                return MiningOp.DeleteMultiClassification;
            }
            else if (query.Command.Delete?.DeleteBinaryClassificationName != null)
            {
                return MiningOp.DeleteBinaryClassification;
            }
            else if (query.Command.Create?.MultiClassification != null)
            {
                return MiningOp.BuildMultiClassification;
            }
            else if (query.Command.Classify != null)
            {
                if (query.Command.Classify.UsingEntries)
                {
                    return MiningOp.ClassifyEntries;
                }
                else 
                    return MiningOp.Classify;
            }

            else if (query.Command.Check != null)
            {
                return MiningOp.Check;
            }
            else if (query.Command.Create?.BinaryClassification != null)
            {
                return MiningOp.BuildBinaryClassification;
            }

            return MiningOp.None;
        }

        // Get and construct the SQL query
        private static string GetSqlString(Query query)
        {
            string command = "";
            if(!string.IsNullOrEmpty(query.Command.From))
            {
                string from = "FROM " + query.Command.From;
                if (query.Command.Create?.MultiClassification != null)
                {
                    var columns = query.Command.Create.MultiClassification.GoalColumn;
                    foreach (var col in query.Command.Create.MultiClassification.InputColumns)
                    {
                        columns += ", " + col;
                    }
                    command = "SELECT " + columns + " ";
                }
                else if (query.Command.Create?.BinaryClassification != null)
                {
                    var columns = query.Command.Create.BinaryClassification.GoalColumn;
                    foreach (var col in query.Command.Create.BinaryClassification.InputColumns)
                    {
                        columns += ", " + col;
                    }
                    command = "SELECT " + columns + " ";
                }
                else
                {
                    command = "SELECT * ";
                }
                command += from;
                
            }
            if (query.Command.Where != null && query.Command.Where.Conditions.Count > 0)
            {
                command += " " + GetWhereCondition(query.Command.Where);
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
