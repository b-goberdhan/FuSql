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
                var path = Path.GetFullPath("./drugs.db");
                var db = new SqliteDb(path);
                db.Connect();

                // Setup a tokenizer, parser, and SQL converter
                var handler = new FUSQLHandler();
                var query = handler.ParseQuery("CHECK FOR sideEffects USING 'Perfectly fine no problems!' WITH sideEffectsReview FROM drugLibTrain\n");
                //var query = handler.ParseQuery("CHECK FOR rating USING 'accutane' WITH urlDrugName FROM drugLibTrain\n");
                //var query = handler.ParseQuery("CHECK 'In pain and want to die' WITH commentsReview AND rating FROM drugLibTrain\n");
                var translation = Translator.TranslateQuery<DrugIssues>(query);
                var result = translation.RunBinaryClassification(db);

                Console.WriteLine($"Sentiment: {translation.Operation.Description} | Prediction: {(Convert.ToBoolean(result.Prediction) ? "Positive" : "Negative")} | Probability: {result.Probability} ");
                Console.ReadLine();
            }
            else if(userInput == "MC")
            {
                Console.WriteLine("MC selected");
                // Connect to the multiclass classification DB
                //var path = Path.GetFullPath("./multiclass.db");
                //var db = new SqliteDb(path);
                //db.Connect();

                //// Setup a tokenizer, parser, and SQL converter
                //var handler = new FUSQLHandler();
                //var query = handler.ParseQuery("IDENTIFY '404 error not found' 'Cant find webpage!' FROM issuestrain\n");
                //var translation = Translator.TranslateQuery<IssueDesc>(query);
                //var result = translation.RunMulticlassClassification(db);

                //Console.WriteLine($"=============== Single Prediction just-trained-model - Result: {result.Area} ===============");

                var path = Path.GetFullPath("./drugs.db");
                var db = new SqliteDb(path);
                db.Connect();

                // Setup a tokenizer, parser, and SQL converter
                var handler = new FUSQLHandler();
                var query = handler.ParseQuery("IDENTIFY sideEffects USING 'accutane' WITH sideEffectsReview FROM drugLibTrain\n");
                var translation = Translator.TranslateQuery<IssueDesc>(query);
                var result = translation.RunMulticlassClassification(db);

                // what is the value of result.Area?
                //Console.WriteLine($"Sentiment: {translation.Operation.Description} | Prediction: {result.Area} | Probability: {result.Probability} ");

                Console.ReadLine();
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

    class DrugIssues
    {
        public string sideEffects { get; set; }
        public string Description { get { return sideEffects; } }
        public string sideEffectsReview { get; set; }
        [ColumnName("Label")]
        public bool GoalTable { get; set; }

        //public int rating { get; set; }
        //public string urlDrugName { get; set; }
        //[ColumnName("Label")]
        //public bool GoalTable { get; set; }

        //public string commentsReview { get; set; }
        //public string sideEffects { get; set; }
        //public string Description { get { return commentsReview; } }
        //public string sideEffectsReview { get; set; }
        //[ColumnName("Label")]
        //public int rating { get; set; }
    }

    class IssueDesc
    {
        public string sideEffects { get; set; }
        public string sideEffectsReview { get; set; }
        //public string Area { get; set; }
        public string Description { get; set; }
        [VectorType(3107)]
        public string[] DescriptionTable { get; set; }
        public string GoalTable { get; set; }
        [ColumnName("Label")]
        public float Label;
    }
}
