using Antlr4.Runtime;
using FUSQL;
using FUSQL.Grammer;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using Database.SQLite;
using FUSQL.Mining;
using FUSQL.SQLTranslate.Translator;
using FUSQL.SQLTranslate.Translator.Extensions;
using FUSQL.SQLTranslate.Results;

namespace FUSQL_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Data-mining operation doesn't use the queried data at the moment
            // Get the user query input, tokenize, and parse it to a SQL data structure
            var path = Path.GetFullPath("./database.sqlite");
            var db = new SqliteDb(path);
            db.Connect();
            var handler = new FUSQLHandler();
            Console.WriteLine("Write a query to get get cluster information:");
            string queryString = "";
            while ((queryString = Console.ReadLine()) != "end")
            {
                //FIND 5 GROUPS irisClusters USING SepalLengthCm PetalLengthCm FROM Iris\n
                var query = handler.ParseQuery("FIND 5 GROUPS lenghts USING SepalLengthCm FROM Iris WHERE PetalLengthCm > 5\n");
                var translation = Translator.TranslateQuery<Iris>(query);
                var resultView = translation.RunClustering(db);

                foreach (var key in (resultView as ClusterResultView<Iris>).Clusters.Keys)
                {
                    Console.WriteLine("Cluster " + key + " count : " + (resultView as ClusterResultView<Iris>).Clusters[key].Count);
                }
            }
            
            Console.ReadLine();
        }
        
    }
    
    class Iris
    {
        public float SepalLengthCm { get; set; }
        public float SepalWidthCm { get; set; }
        public float PetalLengthCm { get; set; }
        public float PetalWidthCm { get; set; }
        public string Species { get; set; }
    }

}
