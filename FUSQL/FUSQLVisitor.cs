using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using FUSQL.Grammer;
using FUSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL
{
    public class FUSQLVisitor : FUSQLBaseVisitor<Query>
    {
        private Query ParsedQuery;
        private int _commandIndex = -1;
        private int _attributeIndex = -1;
        public override Query VisitQuery([NotNull] FUSQLParser.QueryContext context)
        {
            ParsedQuery = new Query();
            _commandIndex = -1;
            _attributeIndex = -1;
            base.VisitQuery(context);
            return ParsedQuery;
            
        }
        public override Query VisitCommand([NotNull] FUSQLParser.CommandContext context)
        {
            ParsedQuery.Commands.Add(new Command()
            {    
            });
            _commandIndex++;
            return base.VisitCommand(context);
        }
        public override Query VisitFind([NotNull] FUSQLParser.FindContext context)
        {
            ParsedQuery.Commands[_commandIndex].Find = new Find();
            return base.VisitFind(context);
        }
        public override Query VisitGroup([NotNull] FUSQLParser.GroupContext context)
        {
            var command = ParsedQuery.Commands[_commandIndex];
            command.Find.Group = new Group()
            {
                Name = context.name().GetText()
            };
            return base.VisitGroup(context);
        }
        public override Query VisitFrom([NotNull] FUSQLParser.FromContext context)
        {
            ParsedQuery.Commands[_commandIndex].Find.From = context.name().GetText();
            return base.VisitFrom(context);
        }
        public override Query VisitAttribute([NotNull] FUSQLParser.AttributeContext context)
        {
            var command = ParsedQuery.Commands[_commandIndex];
            command.Find.Group.Attributes.Add(new Models.Attribute()
            {
                Name = context.name().GetText(),
                Value = context.value().GetText(),
                Operation = context.EQUAL() == null ? "!=" : "=" 
            });
            _attributeIndex++;
            return base.VisitAttribute(context);
        }
    }
}
