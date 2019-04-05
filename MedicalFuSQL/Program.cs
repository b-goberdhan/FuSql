using Database.BaseDb;
using Database.SQLite;
using DataMinner.Mining.Enums;
using DataMinner.Mining.MultiClassification;
using FUSQL;
using FUSQL.Models;
using FUSQL.SQLTranslate.Results;
using FUSQL.SQLTranslate.Translator;
using FUSQL.SQLTranslate.Translator.Extensions;
using MedicalFuSQL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalFuSQL
{
    class Program
    {
        private static Db _db;
        private static FUSQLHandler fusql = new FUSQLHandler();
        static void Main(string[] args)
        {
            _db = LoadMedicalDB();
            _db.Connect();

            Console.WriteLine("Connected to the medical drugs database...");
            Console.WriteLine("You can now enter your query:");
            RunApp();

        }
        private static Db LoadMedicalDB()
        {
            return new SqliteDb(Path.GetFullPath("./drugs.db"));
        }
        private static void RunApp()
        {
            while(true)
            {
                try
                {
                    string queryString = Console.ReadLine();
                    Query query = fusql.ParseQuery(queryString + "\n");
                    
                    Translation<DrugTestModel> translation = Translator.TranslateQuery<DrugTestModel>(query);
                    var result = translation.RunDataMining(_db) as MultiClassification<DrugTestModel>;
                    while(true)
                    {
                        Console.WriteLine("Enter some text as predictions");
                        var prediction = result.Evaluate(new DrugTestModel() { sideEffectsReview = Console.ReadLine() });
                        Console.WriteLine(prediction.GoalTable);
                    }
                    if (translation.Operation.MiningOp == MiningOp.Clustering)
                    {
                        
                    }
                    else if (translation.Operation.MiningOp == MiningOp.BinaryClassification)
                    {

                    }
                    else if (translation.Operation.MiningOp == MiningOp.MultiClassification)
                    {

                    }
                }
                catch(Exception e)
                {
                    break;
                }
            }
        }
        
    }
}
