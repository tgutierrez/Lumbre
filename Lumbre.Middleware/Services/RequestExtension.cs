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
            => await dispatcher.Perform<T, ObjectResponse<T>>(new QueryById<T>(new Interfaces.Common.Primitives.ResourceId(id)));

        public async static Task<IResponse<JsonResponse>> GetJsonById<T>(this IFhirDispatcher dispatcher, string id) where T : IIdentifiable<List<Identifier>>, new() 
            => await dispatcher.Perform<T, JsonResponse>(new QueryById<T>(new Interfaces.Common.Primitives.ResourceId(id)));

        public async static Task<IResponse<Outcome>> PutResource<T>(this IFhirDispatcher dispatcher, T fhirObject, string id) where T : IIdentifiable<List<Identifier>>, new()
            => await dispatcher.Perform<T, Outcome>(new PutRequest<T>(fhirObject, new Interfaces.Common.Primitives.ResourceId(id)));

        public async static Task<IResponse<Outcome>> DeleteResource<T>(this IFhirDispatcher dispatcher, string id) where T : IIdentifiable<List<Identifier>>, new()
            => await dispatcher.Perform<T, Outcome>(new DeleteRequest<T>(new Interfaces.Common.Primitives.ResourceId(id)));
    }
}
