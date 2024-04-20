using Hl7.Fhir.Model;
using MediatR;
using static Lumbre.Interfaces.Common.Primitives;
using Lumbre.Interfaces.Contracts;

namespace Lumbre.Middleware.Requests
{
    internal record QueryByIdRequest(IFHIRRequest RequestContent) : IRequest<IOperationResponse>, IMutableCommand 
    {
        public JsonPayload? JsonPayload { get; set; }
        public DomainResource? RequestResource { get; set; }
    }
}
