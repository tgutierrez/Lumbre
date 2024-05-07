using Lumbre.Interfaces.Query;
using Lumbre.Interfaces.Query.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Services.Misc
{
    public class FHIRExpressionVisitor : ExpressionVisitor
    {
        public List<ISearchExpression> Queries { get; } = new();

        private SimpleExpression _currentSimpleExpression = null;
        internal Stack<ExpressionGroup> Stack { get; set; } = new();


        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType.In(ExpressionType.AndAlso, ExpressionType.OrElse))
            {
                var newGroup = new ExpressionGroup();
                Stack.Push(newGroup);
                newGroup.Op = node.NodeType.ToLogical();
                Visit(node.Left);
                Visit(node.Right);
                Queries.Add(newGroup);
                return node;
            }

            _currentSimpleExpression = new SimpleExpression();
            _currentSimpleExpression.Op = node.NodeType;

            Visit(node.Left);
            Visit(node.Right);
            if (Stack.Count == 0)
            {
                Queries.Add(_currentSimpleExpression);
                return node;
            }
            var current = Stack.Pop();
            current.Expressions.Add(_currentSimpleExpression);
            Stack.Push(current);
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value is Expression value)
            {
                Visit(value);
            }
            else
            {
                _currentSimpleExpression.ExpectedValue = node.Value;
            }
            return node;
        }


        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null)
            {
                Visit(node.Expression);
            }

            var attr = node.Member.GetCustomAttribute<Lumbre.Interfaces.Query.Metadata.JsonPathAttribute>();
            if (attr == null)
                return node;
            var path = attr.Path;

            _currentSimpleExpression.Path = path;

            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            return node;
        }


        protected override Expression VisitNew(NewExpression node)
        {
            // Compile the NewExpression into a lambda, then compile that into a delegate
            var converted = Expression.Convert(node, typeof(object));
            var lambda = Expression.Lambda<Func<object>>(converted);
            var compiled = lambda.Compile();

            // Execute the delegate to create the object
            var newObject = compiled();
            _currentSimpleExpression.ExpectedValue = newObject;
            return node;
        }
    }

    public static class ExtUtils
    {

        public static bool In<T>(this T value, params System.Enum[] values) where T : System.Enum
            => values.Any(v => v.Equals(value));

        public static LogicalGroupOperand ToLogical<T>(this T value) where T : System.Enum => value switch
        {
            ExpressionType.AndAlso => LogicalGroupOperand.And,
            ExpressionType.OrElse => LogicalGroupOperand.Or,
        };
    }
}
