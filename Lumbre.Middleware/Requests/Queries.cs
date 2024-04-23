using Hl7.Fhir.Model;
using MediatR;
using static Lumbre.Interfaces.Common.Primitives;
using Lumbre.Interfaces.Contracts;

namespace Lumbre.Middleware.Requests
{
    internal record QueryByIdRequest<T, K>(QueryById<T> RequestContent) : IRequest<IResponse<K>>, IMutableCommand<T> where T : IIdentifiable<List<Identifier>>, new() where K : IExpectedResponseType
    {
        public JsonPayload? JsonPayload { get; set; }
        public T? RequestResource { get; set; }
    }
}
