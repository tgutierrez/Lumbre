using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Interfaces.Repository;
using Lumbre.Middleware.Requests;
using Lumbre.Middleware.Services.Definition;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lumbre.Interfaces.Common.Primitives;
using Task = System.Threading.Tasks.Task;

namespace Lumbre.Middleware.Handlers.ForQueries
{
    internal class QueryByIdHandler<T, K> : IRequestHandler<QueryByIdRequest<T, K>, IResponse<K>> where T : IIdentifiable<List<Identifier>>, new()
                                                                                                  where K : IExpectedResponseType
    {
        private readonly IShapeResponse<T, K> _response;

        public QueryByIdHandler(IShapeResponse<T, K> response)
        {
            _response = response;
        }

        public Task<IResponse<K>> Handle(QueryByIdRequest<T, K> request, CancellationToken cancellationToken) => Task.FromResult(_response.CreateResponseFrom(request));

     
    }
}
