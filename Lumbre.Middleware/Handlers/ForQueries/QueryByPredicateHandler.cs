using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Interfaces.Query;
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
    internal class QueryByPredicateHandler<T, Q> : IRequestHandler<QueryByPredicateRequest<T, Q>, IResponse<BundledRespose>>
            where T : IIdentifiable<List<Identifier>>, new()
            where Q : IQuery<T>
    {
        private readonly IRepository _repository;

        public QueryByPredicateHandler(IRepository repository)
        {
            _repository = repository;
        }

        public Task<IResponse<BundledRespose>> Handle(QueryByPredicateRequest<T, Q> request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
