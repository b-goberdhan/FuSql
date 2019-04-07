using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using FUSQL.Grammer;
using FUSQL.Models;
using FUSQL.Models.Create;
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
        public override Query VisitCreate([NotNull] FUSQLParser.CreateContext context)
        {
            ParsedQuery.Command.Create = new Create();
      
            return base.VisitCreate(context);
        }
        public override Query VisitClassification([NotNull] FUSQLParser.ClassificationContext context)
        {
            var multiclassification = new MultiClassification();
            multiclassification.Name = context.name().GetText();
            multiclassification.GoalColumn = context.goal().GetText();
            ParsedQuery.Command.Create.MultiClassification = multiclassification;;
            return base.VisitClassification(context);
        }
        public override Query VisitChecker([NotNull] FUSQLParser.CheckerContext context)
        {
            var binaryclassification = new BinaryClassification();
            binaryclassification.Name = context.name().GetText();
            binaryclassification.GoalColumn = context.goal().GetText();
            ParsedQuery.Command.Create.BinaryClassification = binaryclassification;
            return base.VisitChecker(context);
        }
        public override Query VisitDelete([NotNull] FUSQLParser.DeleteContext context)
        {
            ParsedQuery.Command.Delete = new Delete();
            return base.VisitDelete(context);
        }
        public override Query VisitDeleteclassification([NotNull] FUSQLParser.DeleteclassificationContext context)
        {
            
            ParsedQuery.Command.Delete.DeleteClassifactionName = context.name().GetText();
            return base.VisitDeleteclassification(context);
        }
        public override Query VisitClassify([NotNull] FUSQLParser.ClassifyContext context)
        {
            ParsedQuery.Command.Classify = new Classify()
            {
                Terms = new List<Term>(),
                ClassifierName = context.name().GetText()
            };
            return base.VisitClassify(context);
        }
        public override Query VisitTerm([NotNull] FUSQLParser.TermContext context)
        {
            ParsedQuery.Command.Classify.Terms.Add(new Term
            {
                Value = context.@string().GetText(),
                Column = context.column().GetText()
            });
            return base.VisitTerm(context);
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
            if (ParsedQuery.Command.Find != null)
            {
                ParsedQuery.Command.Find.From = context.name().GetText();
            }
            else if (ParsedQuery.Command.Check != null)
            {
                ParsedQuery.Command.Check.From = context.name().GetText();
            }
            else if (ParsedQuery.Command.Identify != null)
            {
                ParsedQuery.Command.Identify.From = context.name().GetText();
            }
            else if (ParsedQuery.Command.Create?.MultiClassification != null)
            {
                ParsedQuery.Command.Create.MultiClassification.From = context.name().GetText();
            }
            else if (ParsedQuery.Command.Create?.BinaryClassification != null)
            {
                ParsedQuery.Command.Create.BinaryClassification.From = context.name().GetText();
            }
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
            if (command.Find != null)
            {
                command.Find.Group.Columns.Add(context.GetText());
            }
            else if (command.Create?.MultiClassification != null)
            {
                command.Create.MultiClassification.InputColumns.Add(context.GetText());
            }
            else if (command.Create?.BinaryClassification != null)
            {
                command.Create.BinaryClassification.InputColumns.Add(context.GetText());
            }
           
            return base.VisitColumn(context);
        }
        public override Query VisitString([NotNull] FUSQLParser.StringContext context)
        {
            if(ParsedQuery.Command.Check != null)
            {
                ParsedQuery.Command.Check.Description = context.GetText();
            }
            
            return base.VisitString(context);
        }
        public override Query VisitCheck([NotNull] FUSQLParser.CheckContext context)
        {
            ParsedQuery.Command.Check = new Check();
            return base.VisitCheck(context);
        }
        public override Query VisitIdentify([NotNull] FUSQLParser.IdentifyContext context)
        {
            ParsedQuery.Command.Identify = new Identify();
            return base.VisitIdentify(context);
        }
        
        public override Query VisitFor([NotNull] FUSQLParser.ForContext context)
        {
            ParsedQuery.Command.Check.GoalTable = context.name().GetText();
            return base.VisitFor(context);
        }
        
        public override Query VisitUsing([NotNull] FUSQLParser.UsingContext context)
        {
            if (ParsedQuery.Command.Check != null)
            {
                ParsedQuery.Command.Check.Description = context.@string().GetText();
            }
            else if (ParsedQuery.Command.Identify != null)
            {
                ParsedQuery.Command.Identify.Description = context.@string().GetText();
            }
            return base.VisitUsing(context);
        }
        public override Query VisitWith([NotNull] FUSQLParser.WithContext context)
        {
            if (ParsedQuery.Command.Check != null)
            {
                ParsedQuery.Command.Check.DescriptionTable = context.name().GetText();
            }
            else if (ParsedQuery.Command.Identify != null)
            {
                ParsedQuery.Command.Identify.DescriptionTable = context.name().GetText();
            }
            
            return base.VisitWith(context);
        }
        public override Query VisitDescription([NotNull] FUSQLParser.DescriptionContext context)
        {
            ParsedQuery.Command.Identify.GoalTable = context.GetText();
            return base.VisitDescription(context);
        }
        public override Query VisitAnd([NotNull] FUSQLParser.AndContext context)
        {
            ParsedQuery.Command.Check.GoalTable = context.GetText();
            return base.VisitAnd(context);
        }
    }
}
