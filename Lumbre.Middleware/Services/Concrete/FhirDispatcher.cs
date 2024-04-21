using Lumbre.Middleware.Services.Definition;
using Hl7.Fhir.Model;
using MediatR;
using Lumbre.Middleware.Requests;
using Lumbre.Interfaces.Contracts;

namespace Lumbre.Middleware.Services.Concrete
{
    internal class FhirDispatcher : IFhirDispatcher
    {
        readonly IMediator _mediator;

        public FhirDispatcher(IMediator mediator)
        {
            this._mediator = mediator;
        }


        public Task<IResponse<K>> Perform<T, K>(IFHIRRequest request)
            where T : IIdentifiable<List<Identifier>>, new()
            where K : IExpectedResponseType
             => _mediator.Send(request switch
             {
                 QueryById<T> => new QueryByIdRequest<T,K>(request as QueryById<T>),
                 _ => throw new NotImplementedException()
             });
    }
}
