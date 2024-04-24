using Hl7.Fhir.Model;
using Hl7.FhirPath.Expressions;
using Lumbre.Interfaces.Contracts;
using Lumbre.Middleware.Handlers.ForQueries;
using Lumbre.Middleware.Requests;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hl7.Fhir.Model.MessageHeader;

namespace Lumbre.Middleware.Utilities
{
    internal static class RegistrationUtilities
    {
        /// <summary>
        /// Kludge: Since I'm having troubles registering open generics, I had to hardcode some types.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="mrequest"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static IServiceCollection RegisterHandlesFor(this IServiceCollection services, Type parameter, Type service, Type implementation)

        {
            // sanity checks
            if (!service.IsGenericType) throw new InvalidOperationException($"{service} is not a generic type");
            if (!implementation.IsGenericType) throw new InvalidOperationException($"{implementation} is not a generic type");

            ResourceAndAllResultTypesIterator((t, k) => services.AddTransient(t, k), parameter, service, implementation);

            return services;
        }

        internal static void ResourceAndAllResultTypesIterator(Action<Type, Type> register, Type pservice, Type prequest, Type pimplementation)
        {
            Limits
                .GetSupportedDocumentTypes()
                .ToList()
                .ForEach(t =>
                {
                    Limits
                        .GetSupportedReturnTypes()
                        .ToList()
                        .ForEach(k =>
                        {
                            var responseType = k.IsGenericType ? k.MakeGenericType(t) : k;
                            (Type irequesthandler, Type concreteHandler) = BuildTypes(pservice, prequest, pimplementation, t, responseType);
                            register(irequesthandler, concreteHandler);
                        });
                });
        }



        internal static void ResourceForResultIterator(Action<Type, Type> register, Type presponseType, Type pservice, Type prequest, Type pimplementation)
        {
            Limits
                .GetSupportedDocumentTypes()
                .ToList()
                .ForEach(t =>
                {
                    var responseType = presponseType.MakeGenericType(t);
                    var request = prequest.MakeGenericType(t, responseType);
                    var iresponse = typeof(IResponse<>).MakeGenericType(responseType);
                    var irequesthandler = pservice.MakeGenericType(request, iresponse);
                    var concreteHandler = pimplementation.MakeGenericType(t);

                    register(irequesthandler, concreteHandler);
                });
        }

        internal static void RequestToResponseService(Action<Type, Type> register, Type presponseType, Type pservice, Type prequest, Type pimplementation)
        {
            Limits
                .GetSupportedDocumentTypes()
                .ToList()
                .ForEach(t =>
                {
                    var responseType = presponseType.IsGenericType? presponseType.MakeGenericType(t) : presponseType;
                    var request = prequest.MakeGenericType(t, responseType);
                    var iresponse = typeof(IResponse<>).MakeGenericType(responseType);
                    var irequesthandler = pservice.MakeGenericType(t, responseType);
                    var concreteHandler = pimplementation.MakeGenericType(t);

                    register(irequesthandler, concreteHandler);
                });
        }

        private static (Type, Type) BuildTypes(Type pservice, Type prequest, Type pimplementation, Type domainType, Type responseType)
        {
            var request = prequest.MakeGenericType(domainType, responseType);
            var iresponse = typeof(IResponse<>).MakeGenericType(responseType);
            var irequesthandler = pservice.MakeGenericType(request, iresponse);
            var concreteHandler = pimplementation.MakeGenericType(domainType, responseType);
            return (irequesthandler, concreteHandler);
        }


    }
}
