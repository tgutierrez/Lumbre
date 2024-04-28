using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection;
using static Lumbre.Frontend.Web.RequestHandlers.Requests;

namespace Lumbre.Frontend.Web.RequestHandlers
{
    public static class RouteBuilder
    {
        public static RouteGroupBuilder MapFHIRRoutes(this RouteGroupBuilder group)
        {
            foreach (var type in Limits.GetSupportedDocumentTypes())
            {
                MapGet(group, type);
                MapPut(group, type);    
            }

            return group;
        }

        private static void MapGet(RouteGroupBuilder group, Type type)
        {
            Type request = typeof(Requests);
            var baseMethod = request.GetMethod("GetSingle");
            var typedMethod = baseMethod.MakeGenericMethod(type);

            group.MapGet($"/{type.Name}/{{id}}", Delegate.CreateDelegate(typeof(GetSingleDelegate<>), typedMethod))
                .WithOpenApi()
                .WithMetadata()
                .WithName($"GET {type.Name} by Id");
        }

        private static void MapPut(RouteGroupBuilder group, Type type)
        {
            Type request = typeof(Requests);
            var baseMethod = request.GetMethod("PutSingle");
            var typedMethod = baseMethod.MakeGenericMethod(type);

            group.MapPut($"/{type.Name}/{{id}}", Delegate.CreateDelegate(typeof(PutSingleDelegate<>).MakeGenericType(type), typedMethod))
                .WithOpenApi()
                .WithMetadata()
                .WithName($"PUT {type.Name} on Id");
        }
    }
}
