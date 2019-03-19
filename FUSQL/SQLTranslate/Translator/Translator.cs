﻿using System;
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

        // Determine the data mining rule/operation
        // This is used in an if-statement by the "RunX" function(s)
        private static MiningOp GetMiningOp(Query query)
        {
            // TODO: query doesn't have anything to indicate which the mining operation
            if (query.Command.Find != null)
            {
                return MiningOp.Clustering;
            }
            return MiningOp.None;
        }

        // Get and construct the SQL query
        private static string GetSqlString(Query query)
        {
            // By default all queries will grab all attributes of a table
            string command = "SELECT * FROM " + query.Command.Find.From;
            return command;
        }

        // Get the cluster/group count from the query 
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
    }
}
