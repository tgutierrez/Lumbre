using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lumbre.Interfaces.Common.Primitives;

namespace Lumbre.Interfaces.Repository;


public record Results(JsonPayload ResultContent) : IRepositoryResult
{
    public JsonPayload? Result => ResultContent;
    public bool HasResult => true;
}


public record NotFound : IRepositoryResult
{
    public bool HasResult => false;
    public JsonPayload? Result => null;
}
