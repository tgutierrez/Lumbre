using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Interfaces.Common
{
    public class Primitives
    {
        public readonly record struct ResourceId(string Id)
        {
            public static implicit operator string(ResourceId id) => id.Id;
        }

        public readonly record struct JsonPayload(string content) 
        {
            public static implicit operator string(JsonPayload response) => response.content;
        }

        public readonly record struct CollectionName(string Name)
        {
            public static implicit operator string(CollectionName collectionName) => collectionName.Name;
        }

        public readonly record struct PagedResults(int PageSize, int PageNumber)
        {
            public PagedResults() : this(25, 1) { }
        }
    }
}
