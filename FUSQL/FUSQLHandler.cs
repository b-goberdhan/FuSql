using Antlr4.Runtime;
using FUSQL.Exceptions;
using FUSQL.Grammer;
using FUSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL
{
    public class FUSQLHandler
    {
        public FUSQLHandler()
        {

        }

        public Query ParseQuery(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            FUSQLLexer lexer = new FUSQLLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            FUSQLParser parser = new FUSQLParser(commonTokenStream);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new ParsingExceptionListener());
            // Specify the root node of the AST AKA query starting pt 
            FUSQLParser.QueryContext query = parser.query(); 

            FUSQLVisitor visitor = new FUSQLVisitor();
            return visitor.VisitQuery(query);
            
        }
    }
}
