using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Middleware.Requests;
using Lumbre.Middleware.Services.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Services.Concrete.ResponseShaping
{
    internal class SingleObjectResponse<T> : IShapeResponse<T, ObjectResponse<T>> where T : IIdentifiable<List<Identifier>>, new()
    {
        public IResponse<ObjectResponse<T>> CreateResponseFrom(IMutableCommand<T> command)
        {
            return new ValidResponse<ObjectResponse<T>>("found", new ObjectResponse<T>(command.RequestResource), command.JsonPayload.Value);
        }
    }
}
