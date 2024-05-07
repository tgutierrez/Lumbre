using Hl7.Fhir.Model;
using Lumbre.Interfaces.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre
{
    public static class QueryBuilder
    {
        public static Expression<Func<K, bool>> On<K>(Expression<Func<K,bool>> query) 
        {
            return query;
        }
    }
}
