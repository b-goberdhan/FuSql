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
            var db = new Database.SQLite.Db(path);
            db.Connect();
            List<Iris> list = new List<Iris>();
            db.Command<Iris>("SELECT * FROM Iris", (iris) =>
            {
                list.Add(iris);
            });
            int y = 0;
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
     
}
