using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Middleware;
using Lumbre.Middleware.Services.Definition;
using Microsoft.AspNetCore.Mvc;

namespace Lumbre.Frontend.Web.RequestHandlers
{
    public static class Requests
    {
        public delegate Task<IResult> GetSingleDelegate<T>(string id, IFhirDispatcher fhirDispatcher) where T : IIdentifiable<List<Identifier>>, new();
        public delegate Task<IResult> PutSingleDelegate<T>([FromBody]T entity, string id, IFhirDispatcher fhirDispatcher) where T : class, IIdentifiable<List<Identifier>>, new();
        public static async Task<IResult> GetSingle<T>(string id, IFhirDispatcher fhirDispatcher) where T: IIdentifiable<List<Identifier>>, new()
        {
            var result = await fhirDispatcher.GetJsonById<T>(id);

            return result switch
            {
                ResourceNotFound<JsonResponse> => TypedResults.NotFound(),
                ValidResponse<JsonResponse> r => TypedResults.Text(r.Response.Value, "application/json"),
                _ => throw new NotImplementedException(),
            };
        }

        public static async Task<IResult> PutSingle<T>([FromBody] DomainResource entity, string id, IFhirDispatcher fhirDispatcher) where T : class, IIdentifiable<List<Identifier>>, new()
        {
            var result = await fhirDispatcher.PutObject(entity as T, id);

            return result switch
            {
                Rejected r => TypedResults.BadRequest(r),
                AcceptedResponse<T> r => TypedResults.Json(r.AcceptedValue),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
