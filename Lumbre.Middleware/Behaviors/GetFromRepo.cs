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
using static Lumbre.Interfaces.Common.Primitives;

namespace Lumbre.Middleware.Behaviors
{
    public class GetFromRepo<T, K> : IPipelineBehavior<QueryByIdRequest<T, K>, IResponse<K>> where T : IIdentifiable<List<Identifier>>, new()
                                                                                        where K : IExpectedResponseType
    {
        private readonly IRepository _repository;
        public GetFromRepo(IRepository repository)
        {
            _repository = repository;
        }

        async Task<IResponse<K>> IPipelineBehavior<QueryByIdRequest<T, K>, IResponse<K>>.Handle(QueryByIdRequest<T, K> request, RequestHandlerDelegate<IResponse<K>> next, CancellationToken cancellationToken)
        {
            var requestContent = request.RequestContent;
            var collectionName = new CollectionName((requestContent.Entity as DomainResource).TypeName);
            var id = requestContent.Id;

            return await _repository.ReadById(collectionName, requestContent.Id) switch
            {
                NotFound => new ResourceNotFound<K>(collectionName, id),
                Results r => await GoNext(r, request, next),
                _ => throw new InvalidOperationException("Unknown Response from Persistance")
            };
        }

        private async Task<IResponse<K>> GoNext(Results r, QueryByIdRequest<T, K> request, RequestHandlerDelegate<IResponse<K>> next)
        {
            request.JsonPayload = r.Result;
            return await next();
        }
    }
}
