using Lumbre.Interfaces.Common;
using Lumbre.Interfaces.Contracts;
using Lumbre.Middleware.Requests;
using Lumbre.Middleware.Services.Concrete;
using Lumbre.Middleware.Services.Definition;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware
{
    public static class Registration
    {
        public static IServiceCollection AddLumbre(this IServiceCollection services, Action<ConfigurationOptions> configure)
        {
            #region Configuration
            var options = new ConfigurationOptions();
            configure?.Invoke(options);
            services.AddScoped(f => options.GetConfiguration());
            #endregion

            #region Service Registration

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<Marker>();
                
            });

            services.AddScoped<IFhirDispatcher, FhirDispatcher>();

            #endregion

            return services;
        }
    }
}
