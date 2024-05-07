using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Interfaces.Query.Metadata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonPathAttribute : Attribute
    {
        public JsonPathAttribute(string path, params ExpressionType[] allowedExpressionTypes)
        {
            this.Path = path;
            this.AllowedExpressionTypes = allowedExpressionTypes;
        }
        public string Path { get; internal set; }
        public IEnumerable<ExpressionType> AllowedExpressionTypes { get; internal set; }

    }
}
