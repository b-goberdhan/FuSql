using Antlr4.Runtime.Misc;
using FUSQL.Grammer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL
{
    public class FUSQLVisitor : FUSQLBaseVisitor<object>
    {
        public override object VisitCommand([NotNull] FUSQLParser.CommandContext context)
        {
            var name = context.group()?.name()?.GetText();
            var attributeName = context.group().attribute()[0]?.name().GetText();
            var attributeValue = context.group().attribute()[0]?.value().GetText();
            var x = context.group().attribute()[0].EQUAL();
            var y = context.group().attribute()[0].NOT_EQUAL();
            return base.VisitCommand(context);
        }
    }
}
