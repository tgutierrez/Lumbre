using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Middleware;
using Lumbre.Middleware.Services.Definition;

namespace Lumbre.Frontend.Web.RequestHandlers
{
    public static class Requests
    {
        public delegate Task<IResult> GetSingleDelegate<T>(string id, IFhirDispatcher fhirDispatcher) where T : IIdentifiable<List<Identifier>>, new();

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
    }
}
