using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Interfaces.Query;
using Lumbre.Interfaces.Query.Descriptors;
using Lumbre.Middleware.Services.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Utilities
{
    public static class ExpressionParser
    {
        public static ISearchExpression[] Parse<T, Q>(this Expression<Func<Q, bool>> predicate) where T : IIdentifiable<List<Identifier>>, new() where Q : IQuery<T>
        {
            var visitor = new FHIRExpressionVisitor();
            visitor.Visit(predicate);
            return visitor.Queries.ToArray();
        }
    }
}
