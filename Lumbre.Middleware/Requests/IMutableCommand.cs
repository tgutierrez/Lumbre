using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lumbre.Interfaces.Common.Primitives;

namespace Lumbre.Middleware.Requests
{
    internal interface IMutableCommand<T> where T : IIdentifiable<List<Identifier>>, new ()
    {
        JsonPayload? JsonPayload { get; set; } 
        T? RequestResource { get; set; }
    }
}
