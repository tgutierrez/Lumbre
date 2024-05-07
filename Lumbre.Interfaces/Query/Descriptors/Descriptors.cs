using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Interfaces.Query.Descriptors
{
    /// <summary>
    /// Definition to describe a search term
    /// </summary>
    public interface ISearchExpression;

    /// <summary>
    /// Simple Expression consisting on an Operand, a JSONPath and an Expected Value
    /// </summary>
    public class SimpleExpression() : ISearchExpression
    {
        /// <summary>
        /// Operator
        /// </summary>
        public ExpressionType Op { get; set; }
        /// <summary>
        /// Json Path
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Holds the Expected Value of the criteria, to be used as query predicate
        /// </summary>
        public Object ExpectedValue { get; set; }
    }

    /// <summary>
    /// Group of search Expressions e.g: ( Expresion 1 && Expression 2 && Expression 3)
    /// </summary>
    public class ExpressionGroup() : ISearchExpression
    {
        /// <summary>
        /// Logical Groups of operations, 
        /// </summary>
        public LogicalGroupOperand Op { get; set; }

        /// <summary>
        /// List of expressions contained under the same logical operator
        /// </summary>
        public IList<ISearchExpression> Expressions { get; set; } = new List<ISearchExpression>();
    };
}
