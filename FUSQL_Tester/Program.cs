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

namespace FUSQL_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            AntlrInputStream input = new AntlrInputStream("FIND GROUPS fireclusters USING hair = ugly skin = pale FROM people\n");
            FUSQLLexer lexer = new FUSQLLexer(input);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            FUSQLParser parser = new FUSQLParser(commonTokenStream);
            FUSQLVisitor visitor = new FUSQLVisitor();
            
            FUSQLParser.QueryContext commandContext = parser.query();
            
            var query = visitor.Visit(commandContext);
            dbSQLite();
            Console.WriteLine(query);
            Console.ReadLine();
        }
        private static void dbSQLite()
        {
            var path = Path.GetFullPath("./database.sqlite");
            var db = new SqliteDb(path);
            db.Connect();
            List<IrisPredict> listPredict = new List<IrisPredict>();
            db.Command<Iris>("SELECT * FROM Iris", (iris) =>
            {
                listPredict.Add(new IrisPredict()
                {
                    PetalLengthCm = Convert.ToSingle(iris.PetalLengthCm),
                    PetalWidthCm = Convert.ToSingle(iris.PetalWidthCm),
                    SepalLengthCm = Convert.ToSingle(iris.SepalLengthCm),
                    SepalWidthCm = Convert.ToSingle(iris.SepalWidthCm),
                    Species = iris.Species
                });
            });
            var clusterer = new Clustering<IrisPredict>(5, listPredict);
            clusterer.BuildModel("SepalLengthCm");
            foreach (var item in listPredict)
            {
                var result = clusterer.Evaluate(item);
                Console.WriteLine(result.SelectedClusterId);
            }
        }
    }
    class Iris
    {
        public decimal SepalLengthCm { get; set; }
        public decimal SepalWidthCm { get; set; }
        public decimal PetalLengthCm { get; set; }
        public decimal PetalWidthCm { get; set; }
        public string Species { get; set; }
    }
    class IrisPredict
    {
        public float SepalLengthCm { get; set; }
        public float SepalWidthCm { get; set; }
        public float PetalLengthCm { get; set; }
        public float PetalWidthCm { get; set; }
        public string Species { get; set; }
    }

}
