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
using Lumbre.Middleware.Behaviors;
using Lumbre.Middleware.Services.Concrete.ResponseShaping;
using Lumbre.Middleware.Handlers.ForCommands;

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

                // Handlers
                RegistrationUtilities.ResourceAndAllResultTypesIterator((t, k) => services.AddScoped(t, k), typeof(IRequestHandler<,>), typeof(QueryByIdRequest<,>), typeof(QueryByIdHandler<,>));
                RegistrationUtilities.RequestToResponseServiceFixedReturn((t, k) => services.AddScoped(t, k), typeof(Outcome), typeof(IRequestHandler<,>),  typeof(PutRequestCommand<>), typeof(PutRequestCommandHandler<>));

                // Pipelines
                RegistrationUtilities.ResourceAndAllResultTypesIterator((t, k) => cfg.AddBehavior(t, k), typeof(IPipelineBehavior<,>), typeof(QueryByIdRequest<,>), typeof(ValidateId<,>));
                RegistrationUtilities.ResourceAndAllResultTypesIterator((t, k) => cfg.AddBehavior(t, k), typeof(IPipelineBehavior<,>), typeof(QueryByIdRequest<,>), typeof(GetFromRepo<,>));
                RegistrationUtilities.ResourceForResultIterator((t, k) => cfg.AddBehavior(t, k), typeof(ObjectResponse<>),typeof(IPipelineBehavior<,>), typeof(QueryByIdRequest<,>), typeof(Deserialize<>));
                RegistrationUtilities.RequestToResponseServiceFixedReturn((t, k) => cfg.AddBehavior(t, k), typeof(Outcome), typeof(IPipelineBehavior<,>), typeof(PutRequestCommand<>), typeof(ValidateObject<>));
                RegistrationUtilities.RequestToResponseServiceFixedReturn((t, k) => cfg.AddBehavior(t, k), typeof(Outcome), typeof(IPipelineBehavior<,>), typeof(PutRequestCommand<>), typeof(SerializeForPut<>));

                // Behaviors
                RegistrationUtilities.RequestToResponseService((t, k) => services.AddScoped(t, k), typeof(ObjectResponse<>), typeof(IShapeResponse<,>), typeof(QueryByIdRequest<,>), typeof(SingleObjectResponse<>));
                RegistrationUtilities.RequestToResponseService((t, k) => services.AddScoped(t, k), typeof(JsonResponse), typeof(IShapeResponse<,>), typeof(QueryByIdRequest<,>), typeof(SingleJsonResponse<>));
            });

            


            services.AddScoped<IFhirDispatcher, FhirDispatcher>();

            #endregion

            return services;
        }
    }

    internal record Configurator(IServiceCollection Services) : IConfigurator;
}
