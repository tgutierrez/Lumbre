using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Middleware.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Services.Definition
{
    internal interface IShapeResponse<T, K> where T : IIdentifiable<List<Identifier>>, new()
                                               where K : IExpectedResponseType
    {
        IResponse<K> CreateResponseFrom(IMutableCommand<T> command);
    }
}
