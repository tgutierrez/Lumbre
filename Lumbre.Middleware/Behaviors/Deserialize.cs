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
    internal class Deserialize<T> : IPipelineBehavior<QueryByIdRequest<T, ObjectResponse<T>>, IResponse<ObjectResponse<T>>> where T : IIdentifiable<List<Identifier>>, new()

    {
        public async Task<IResponse<ObjectResponse<T>>> Handle(QueryByIdRequest<T, ObjectResponse<T>> request, RequestHandlerDelegate<IResponse<ObjectResponse<T>>> next, CancellationToken cancellationToken)
        {
            if (request.JsonPayload == null)
            {
                return new ErrorResponse<ObjectResponse<T>>("Nothing to desearialize. Check Data availability", []);
            }
            
            var options = SerializerOptions.Get();
            request.RequestResource = JsonSerializer.Deserialize<T>(request.JsonPayload, options);

            return await next();
        }
    }
}
