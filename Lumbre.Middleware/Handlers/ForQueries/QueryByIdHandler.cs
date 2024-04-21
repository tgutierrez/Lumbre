using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Interfaces.Repository;
using Lumbre.Middleware.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Handlers.ForQueries
{
    internal class QueryByIdHandler<T, K> : IRequestHandler<QueryByIdRequest<T, K>, IResponse<K>> where T : IIdentifiable<List<Identifier>>, new() 
                                                                                                  where K : IExpectedResponseType
    {
        private readonly IRepository _repositories;
        public QueryByIdHandler(IRepository repositories)
        {
            _repositories = repositories;
        }

        public Task<IResponse<K>> Handle(QueryByIdRequest<T, K> request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}
