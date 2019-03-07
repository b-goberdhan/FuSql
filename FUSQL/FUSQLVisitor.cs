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
        public override Query VisitGroup([NotNull] FUSQLParser.GroupContext context)
        {
            var command = ParsedQuery.Commands[_commandIndex];
            command.Group = new Group()
            {
                Name = context.name().GetText()
            };
            return base.VisitGroup(context);
        }
        public override Query VisitAttribute([NotNull] FUSQLParser.AttributeContext context)
        {
            var command = ParsedQuery.Commands[_commandIndex];
            command.Group.Attributes.Add(new Models.Attribute()
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
