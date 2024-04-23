using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Middleware.Services.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware
{
    public static class RequestExtension
    {
        public async static Task<IResponse<ObjectResponse<T>>> GetObjectById<T>(this IFhirDispatcher dispatcher, string id) where T : IIdentifiable<List<Identifier>>, new()
        {
            return await dispatcher.Perform<T, ObjectResponse<T>>(new QueryById<Patient>(new Interfaces.Common.Primitives.ResourceId(id)));
        }
    }
}
