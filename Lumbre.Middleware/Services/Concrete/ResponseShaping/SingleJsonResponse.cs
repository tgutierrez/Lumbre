using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Middleware.Requests;
using Lumbre.Middleware.Services.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Services.Concrete.ResponseShaping
{
    internal class SingleJsonResponse<T> : IShapeResponse<T, JsonResponse> where T : IIdentifiable<List<Identifier>>, new()
    {
        IResponse<JsonResponse> IShapeResponse<T, JsonResponse>.CreateResponseFrom(IMutableCommand<T> command)
                                 => new ValidResponse<JsonResponse>("found", new JsonResponse(command.JsonPayload.Value), command.JsonPayload.Value);
    }
}
