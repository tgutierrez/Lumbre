using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lumbre.Interfaces.Common.Primitives;

namespace Lumbre.Interfaces.Repository
{
    public interface IRepository
    {
        Task Upsert(CollectionName collection, ResourceId resourceId, JsonPayload payload);
        Task<IRepositoryResult> ReadById(CollectionName collectionName, ResourceId resourceId);
    }
}
