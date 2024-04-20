using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lumbre.Interfaces.Common.Primitives;

namespace Lumbre.Interfaces
{
    public interface IRepository
    {
        void Upsert(CollectionName collection, ResourceId resourceId, JsonPayload payload);
        JsonPayload ReadById(CollectionName collection, ResourceId resourceId);
    }
}
