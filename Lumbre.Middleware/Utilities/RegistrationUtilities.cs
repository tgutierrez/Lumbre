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
        internal static IServiceCollection RegisterHandlesFor(this IServiceCollection services, Type mrequest, Type handler)
                                        
        {
            // sanity checks
            if (!mrequest.IsGenericType) throw new InvalidOperationException($"{mrequest} is not a generic type");
            if (!handler.IsGenericType) throw new InvalidOperationException($"{handler} is not a generic type");

            Limits
                .GetSupportedDocumentTypes()
                .ToList()
                .ForEach(t =>
                {
                    Limits
                        .GetSupportedReturnTypes()
                        .Where(k => k.IsGenericType)
                        .ToList()
                        .ForEach(k =>
                        {
                            var responseType = k.MakeGenericType(t);
                            var request = mrequest.MakeGenericType(t, responseType);
                            var iresponse = typeof(IResponse<>).MakeGenericType(responseType);
                            var irequesthandler = typeof(IRequestHandler<,>).MakeGenericType(request, iresponse);
                            var concreteHandler = handler.MakeGenericType(t, responseType);

                            services.AddTransient(irequesthandler, concreteHandler);
                        });
                });

            //services.AddTransient(typeof(IRequestHandler<,>)
            //              .MakeGenericType([
            //                  typeof(QueryByIdRequest<,>)
            //                                .MakeGenericType(new Type[]
            //                                {
            //                                     typeof(Patient),
            //                                     typeof(ObjectResponse<>)
            //                                        .MakeGenericType(new []{
            //                                            typeof(Patient)
            //                                        })
            //                                }),
            //                            typeof(IResponse<>)
            //                                .MakeGenericType(new Type[]{
            //                                    typeof(ObjectResponse<>)
            //                                        .MakeGenericType(new []{
            //                                            typeof(Patient)
            //                                        })
            //                                })
            //                            ,
            //              ])
            //             , typeof(QueryByIdHandler<,>)
            //                  .MakeGenericType(new Type[]
            //                      {
            //                                     typeof(Patient),
            //                                     typeof(ObjectResponse<>)
            //                                        .MakeGenericType(new []{
            //                                            typeof(Patient)
            //                                        })
            //                      })
            //             );

            return services;
        }
    }
}
