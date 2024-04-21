using Hl7.Fhir.Model;
using Lumbre.Interfaces.Common;
using Lumbre.Interfaces.Contracts;
using Lumbre.Middleware;
using Lumbre.Middleware.Handlers.ForQueries;
using Lumbre.Middleware.Requests;
using Lumbre.Middleware.Services.Concrete;
using Lumbre.Middleware.Services.Definition;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Lumbre.Middleware.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre
{
    public static class Registration
    {
        public static IServiceCollection AddLumbre(this IServiceCollection services, Action<IConfigurator> configure)
        {
            #region Configuration
            configure?.Invoke(new Configurator(services));
            #endregion

            #region Service Registration

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<Marker>();
            });


            //services.AddTransient<IRequestHandler<,>
            //services.AddTransient<IRequestHandler<,>, QueryByIdHandler<,>>();
            //services.AddTransient<IRequestHandler<QueryByIdRequest<Patient, ObjectResponse<Patient>>, IResponse<ObjectResponse<Patient>>>, QueryByIdHandler<Patient, ObjectResponse<Patient>>>();
            //services.AddTransient(typeof(IRequestHandler<,>)
            //                        .MakeGenericType([
            //                            typeof(QueryByIdRequest<,>)
            //                                .MakeGenericType(new Type[]
            //                                {
            //                                     typeof(IIdentifiable<List<Identifier>>),
            //                                     typeof(IExpectedResponseType)
            //                                }),
            //                            typeof(IResponse<>)
            //                                .MakeGenericType(new Type[]{
            //                                    typeof(IExpectedResponseType)
            //                                })
            //                            ,
            //                        ])
            //                       , typeof(QueryByIdHandler<,>)
            //                            .MakeGenericType(new Type[]
            //                                {
            //                                     typeof(IIdentifiable<List<Identifier>>),
            //                                     typeof(IExpectedResponseType)
            //                                })
            //                       );
            //services.AddTransient(typeof(IRequestHandler<,>)
            //                        .MakeGenericType([
            //                            typeof(QueryByIdRequest<,>)
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
            //                        ])
            //                       , typeof(QueryByIdHandler<,>)
            //                            .MakeGenericType(new Type[]
            //                                {
            //                                     typeof(Patient),
            //                                     typeof(ObjectResponse<>)
            //                                        .MakeGenericType(new []{
            //                                            typeof(Patient)
            //                                        })
            //                                })
            //                       );
            services.RegisterHandlesFor(typeof(QueryByIdRequest<,>), typeof(QueryByIdHandler<,>));

            //services.RegisterHandlesFor<QueryByIdRequest<,>, QueryByIdRequest<,>);

            services.AddScoped<IFhirDispatcher, FhirDispatcher>();

            #endregion

            return services;
        }
    }

    internal record Configurator(IServiceCollection Services) : IConfigurator;
}
