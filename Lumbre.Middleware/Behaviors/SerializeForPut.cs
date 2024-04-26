using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Middleware.Requests;
using Lumbre.Middleware.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Behaviors
{
    internal class SerializeForPut<T> : IPipelineBehavior<PutRequestCommand<T>, IResponse<Outcome>> where T : IIdentifiable<List<Identifier>>, new()
    {
        public async Task<IResponse<Outcome>> Handle(PutRequestCommand<T> request, RequestHandlerDelegate<IResponse<Outcome>> next, CancellationToken cancellationToken)
        {
            var options = SerializerOptions.Get();
            request.JsonPayload = new Interfaces.Common.Primitives.JsonPayload(JsonSerializer.Serialize<T>(request.RequestContent.Resource, options));

            return await next();
        }
    }
}
