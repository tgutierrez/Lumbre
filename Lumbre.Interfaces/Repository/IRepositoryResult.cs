using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lumbre.Interfaces.Common.Primitives;

namespace Lumbre.Interfaces.Repository
{
    public interface IRepositoryResult
    {
        bool HasResult { get; }
        JsonPayload? Result { get; }
    }
}
