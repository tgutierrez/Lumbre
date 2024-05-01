using Hl7.Fhir.Model;
using Lumbre.Interfaces.Common;
using Lumbre.Interfaces.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Requests
{
    internal record PutRequestCommand<T>(PutRequest<T> RequestContent) : IRequest<IResponse<Outcome>>, IMutableCommand<T> where T : IIdentifiable<List<Identifier>>, new()
    {
        public Primitives.JsonPayload? JsonPayload { get; set; }
        public T? RequestResource { get; set; }
    }

    internal record DeleteRequestCommand<T>(DeleteRequest<T> RequestContent) : IRequest<IResponse<Outcome>>, IMutableCommand<T> where T : IIdentifiable<List<Identifier>>, new()
    {
        public Primitives.JsonPayload? JsonPayload { get; set; }
        public T? RequestResource { get; set; }
    }
}
