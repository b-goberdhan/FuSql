using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.Exceptions
{
    public class ParsingException : Exception
    {
        public string ErrorMessage { get; set; }
    }
    public class ParsingExceptionListener : BaseErrorListener
    {
        public override void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            throw new ParsingException()
            {
                ErrorMessage = "Syntax Error at line : " + e.OffendingToken.Line + "," + e.OffendingToken.Column + " " + msg
            };
        }
    }
}
