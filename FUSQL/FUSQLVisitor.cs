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
        private int _attributeIndex = -1;
        public override Query VisitQuery([NotNull] FUSQLParser.QueryContext context)
        {
            ParsedQuery = new Query();
            _attributeIndex = -1;
            base.VisitQuery(context);
            return ParsedQuery;
        }
        public override Query VisitCommand([NotNull] FUSQLParser.CommandContext context)
        {
            ParsedQuery.Command = new Command();
            return base.VisitCommand(context);
        }
        public override Query VisitFind([NotNull] FUSQLParser.FindContext context)
        {
            ParsedQuery.Command.Find = new Find();
            return base.VisitFind(context);
        }
        public override Query VisitGroups([NotNull] FUSQLParser.GroupsContext context)
        {
            var command = ParsedQuery.Command;
            command.Find.Group = new Group()
            {
                Count = int.Parse(context.number().GetText()),
                Name = context.name().GetText(),

            };
            return base.VisitGroups(context);
        }
        public override Query VisitFrom([NotNull] FUSQLParser.FromContext context)
        {
            ParsedQuery.Command.Find.From = context.name().GetText();
            return base.VisitFrom(context);
        }
        public override Query VisitColumn([NotNull] FUSQLParser.ColumnContext context)
        {
            var command = ParsedQuery.Command;
            command.Find.Group.Columns.Add(context.GetText());
            return base.VisitColumn(context);
        }

    }
}
