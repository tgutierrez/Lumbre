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

        public Task<IOperationResponse> Perform<T>(IFHIRRequest request) where T : IIdentifiable<List<Identifier>>, new()
            => _mediator.Send(request switch
            {
                QueryById<T> => new QueryByIdRequest(request as QueryById<T>),
                _ => throw new NotImplementedException()
            });

    }
}
