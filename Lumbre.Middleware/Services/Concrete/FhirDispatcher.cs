using Lumbre.Middleware.Services.Definition;
using Hl7.Fhir.Model;
using MediatR;
using Lumbre.Middleware.Requests;
using Lumbre.Interfaces.Contracts;
using Lumbre.Interfaces.Query;

namespace Lumbre.Middleware.Services.Concrete
{
    internal class FhirDispatcher : IFhirDispatcher
    {
        readonly IMediator _mediator;

        public FhirDispatcher(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task<IResponse<K>> Perform<T, K>(IFHIRRequest request)
            where T : IIdentifiable<List<Identifier>>, new()
            where K : IExpectedResponseType
             => (request switch
             {
                 QueryById<T> => await _mediator.Send(new QueryByIdRequest<T,K>(request as QueryById<T>)),
                 PutRequest<T> => (IResponse<K>)await _mediator.Send(new PutRequestCommand<T>(request as PutRequest<T>)),
                 DeleteRequest<T> => (IResponse<K>)await _mediator.Send(new DeleteRequestCommand<T>(request as DeleteRequest<T>)),

                 _ => throw new NotImplementedException()
             });

        public async Task<IResponse<K>> PerformQuery<T, K, Q>(IFHIRRequest request)
            where T : IIdentifiable<List<Identifier>>, new()
            where K : IExpectedResponseType
            where Q : IQuery<T>
             => (IResponse<K>)await _mediator.Send(new QueryByPredicateRequest<T, Q>(request as QueryByPredicate<T, Q>));
    }
}
