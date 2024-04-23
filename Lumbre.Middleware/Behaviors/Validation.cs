using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Middleware.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Behaviors
{
    public class ValidateId<T, K> : IPipelineBehavior<QueryByIdRequest<T, K>, IResponse<K>> where T : IIdentifiable<List<Identifier>>, new()
                                                                                            where K : IExpectedResponseType
    {
        async Task<IResponse<K>> IPipelineBehavior<QueryByIdRequest<T, K>, IResponse<K>>.Handle(QueryByIdRequest<T, K> request, RequestHandlerDelegate<IResponse<K>> next, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(request.RequestContent.Id))
            {
                return new ErrorResponse<K>($"id for {typeof(T).Name} must be not null or empty", [""]);
            }

            return await next();
        }
    }
}
