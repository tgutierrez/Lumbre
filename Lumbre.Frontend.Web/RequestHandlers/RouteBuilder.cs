using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection;
using static Lumbre.Frontend.Web.RequestHandlers.Requests;

namespace Lumbre.Frontend.Web.RequestHandlers
{
    public static class RouteBuilder
    {
        public static RouteGroupBuilder MapFHIRRoutes(this RouteGroupBuilder group)
        {
            Type request = typeof(Requests);

            var baseMethod = request.GetMethod("GetSingle");

            foreach (var type in Limits.GetSupportedDocumentTypes())
            {
                MapGet(group, baseMethod, type);

            }

            return group;
        }

        private static void MapGet(RouteGroupBuilder group, MethodInfo? baseMethod, Type type)
        {
            var typedMethod = baseMethod.MakeGenericMethod(type);

            group.MapGet($"/{type.Name}/{{id}}", Delegate.CreateDelegate(typeof(GetSingleDelegate<>), typedMethod))
                .WithOpenApi()
                .WithMetadata()
                .WithName($"GET {type.Name} by Id");
        }
    }
}
