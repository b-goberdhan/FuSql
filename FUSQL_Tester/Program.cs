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
using DataMinner.Mining;
using Microsoft.Data.DataView;

namespace FUSQL_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            // Choose the data mining operation from input
            Console.WriteLine("Input BC for binary classification, MC for multiclass classification, or C for clustering");
            var userInput = Console.ReadLine();
            if(userInput == "BC")
            {
                Console.WriteLine("BC selected");
                // Connect to the binary classification DB
                var path = Path.GetFullPath("./binaryclassification.db"); 
                var db = new SqliteDb(path);
                db.Connect();

                // Setup a tokenizer, parser, and SQL converter
                var handler = new FUSQLHandler();
                var query = handler.ParseQuery("CHECK 'PRETTY GOOD!' FROM imdblabelled\n");
                var translation = Translator.TranslateQuery<Text>(query);
                var result = translation.RunBinaryClassification(db);

                Console.WriteLine($"Sentiment: {translation.Operation.Text} | Prediction: {(Convert.ToBoolean(result.Prediction) ? "Positive" : "Negative")} | Probability: {result.Probability} ");
                Console.ReadLine();
            }
            else if(userInput == "MC")
            {
                Console.WriteLine("MC selected");


                // Setup a tokenizer, parser, and SQL converter
                // var handler = new FUSQLHandler();
                // var query = handler.ParseQuery("");
                // var translation = Translator.TranslateQuery<Iris>(query);
                // var resultView = translation.RunClustering(db);

                var resultView = new MulticlassClassification();
                resultView.BuildModel();
            }
            else if(userInput == "C")
            {
                Console.WriteLine("C selected");
                // Connect to the database
                var path = Path.GetFullPath("./database.sqlite");
                var db = new SqliteDb(path);
                db.Connect();

                // Setup a tokenizer, parser, and SQL converter
                var handler = new FUSQLHandler();
                var query = handler.ParseQuery("FIND 5 GROUPS irisClusters USING SepalLengthCm PetalLengthCm FROM Iris\n");
                var translation = Translator.TranslateQuery<Iris>(query);
                var resultView = translation.RunClustering(db);

                // Perform a data mining query operation
                // dbSQLite();
                foreach (var key in (resultView as ClusterResultView<Iris>).Clusters.Keys)
                {
                    Console.WriteLine("Cluster " + key + " count : " + (resultView as ClusterResultView<Iris>).Clusters[key].Count);
                }
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("\nInvalid input.");
            }
        }
        private static void dbSQLite()
        {
            // Connect to the database
            var path = Path.GetFullPath("./database.sqlite");
            var db = new SqliteDb(path);
            db.Connect();

            // Populate a list of iris from the database 
            List<Iris> listPredict = new List<Iris>();
            db.Command<Iris>("SELECT * FROM Iris", (iris) =>
            {
                listPredict.Add(iris);
            });

            // Perform the clustering data mining operation based on attribute(s)
            var clusterer = new Clustering<Iris>(5, listPredict);
            clusterer.BuildModel("SepalLengthCm");
            // For each item, predict and output which cluster it's in
            foreach (var item in listPredict)
            {
                // Each item is tested against the test data set
                var result = clusterer.Evaluate(item);
                Console.WriteLine(result.SelectedClusterId);
                Console.WriteLine(string.Join(" ", result.Distance));
            }
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

    class Text
    {
        public string SentimentText { get; set; }
        public bool Sentiment { get; set; }
    }
}
