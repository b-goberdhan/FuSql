using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using FUSQL.Grammer;
using FUSQL.Models;
using FUSQL.Models.Enums;
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
        public override Query VisitQuery([NotNull] FUSQLParser.QueryContext context)
        {
            ParsedQuery = new Query();
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
        public override Query VisitGet([NotNull] FUSQLParser.GetContext context)
        {
            var datasetInfo = context.dataset_info().GetText();
            
            Get get;
            if (Enum.TryParse(datasetInfo, out get) && get != Get.None)
            {
                ParsedQuery.Command.Get = get;
            }
           
            return base.VisitGet(context);
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
        public override Query VisitWhere([NotNull] FUSQLParser.WhereContext context)
        {
            ParsedQuery.Command.Find.Where = new Where();
            ParsedQuery.Command.Find.Where.Conditions = new List<Condition>();
            return base.VisitWhere(context);
        }
        public override Query VisitConditions([NotNull] FUSQLParser.ConditionsContext context)
        {
            Condition condition = new Condition();
            condition.ColumnName = context.name().GetText();
            condition.Value = context.value().GetText();
            condition.Operation = ParseOperation(context);
            ParsedQuery.Command.Find.Where.Conditions.Add(condition);
            return base.VisitConditions(context);
        }
        private Operation ParseOperation(FUSQLParser.ConditionsContext context)
        {
            if (context.EQUAL() != null) return Operation.Equal;
            if (context.NOT_EQUAL() != null) return Operation.NotEqual;
            if (context.GREATER_THAN() != null) return Operation.GreaterThan;
            if (context.LESS_THAN() != null) return Operation.LessThan;
            return Operation.None;
        }
        public override Query VisitColumn([NotNull] FUSQLParser.ColumnContext context)
        {
            var command = ParsedQuery.Command;
            command.Find.Group.Columns.Add(context.GetText());
            return base.VisitColumn(context);
        }

    }
}
