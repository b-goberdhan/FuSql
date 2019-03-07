using Antlr4.Runtime;
using FUSQL;
using FUSQL.Grammer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            AntlrInputStream input = new AntlrInputStream("FIND GROUP jim USING hair = ugly\n");
            FUSQLLexer lexer = new FUSQLLexer(input);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            FUSQLParser parser = new FUSQLParser(commonTokenStream);
            FUSQLVisitor visitor = new FUSQLVisitor();
            FUSQLParser.CommandContext commandContext = parser.command();
            
            visitor.Visit(commandContext);
        }
    }
}
