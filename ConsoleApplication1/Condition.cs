using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    internal interface ICondition
    {
    }

    internal abstract class Condition : ICondition
    {
        public Or Or(params Condition[] conditions)
        {
            var orCondition = this as Or;
            if (orCondition == null) 
                return new Or(this, conditions);

            orCondition.Conditions.AddRange(conditions);
            return orCondition;
        }
        public And And(params Condition[] conditions)
        {
            var andCondition = this as And;
            if (andCondition == null)
                return new And(this, conditions);

            andCondition.Conditions.AddRange(conditions);
            return andCondition;
        }
    }

    internal class CompareCondition : Condition
    {
        public Expression LeftExpression { get; set; }
        public CompareOperator Operator { get; set; }
        public Expression RightExpression { get; set; }
    }

    internal abstract class LogicalCondition : Condition
    {
        public List<Condition> Conditions { get; protected set; }
        protected LogicalOperator Operator { get; set; }
    }

    internal sealed class And : LogicalCondition
    {
        public And(Condition condition, params Condition[] conditions)
        {
            Operator = LogicalOperator.And;
            Conditions = new List<Condition> { condition };
            Conditions.AddRange(conditions);
        }
    }

    internal sealed class Or : LogicalCondition
    {
        public Or(Condition condition, params Condition[] conditions)
        {
            Operator = LogicalOperator.Or;
            Conditions = new List<Condition> { condition };
            Conditions.AddRange(conditions);
        }
    }

    enum CompareOperator { Equals, GreaterThan, LessThan, GreaterOrEqual, LessOrEqual, NotEqual, Contains }
    enum LogicalOperator { And, Or }

    internal class ConditionCreator
    {
        public Condition Create()
        {
            CompareCondition c2, c3, c4, c5;
            var c1 = c2 = c3 = c4 = c5 = null;
            return (c1.And(c2).And(c3)).Or(c4.And(c5));
        }
    }
}
