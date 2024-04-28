using Lumbre.Frontend.Web.Configuration;
using System.Net;
using Lumbre.Frontend.Web.RequestHandlers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json;
using Lumbre.Middleware.Utilities;

namespace Lumbre
{
    public static class Configure
    {
        public static IHostBuilder UseLumbreWebFrontend(this IHostBuilder hostBuilder, Action<Configuration> configurationAction)
        {
            hostBuilder.ConfigureWebHostDefaults(web =>
                 {
                     web.UseLumbreWebFrontend(configurationAction);
                 });
            return hostBuilder;
        }

        public static IWebHostBuilder UseLumbreWebFrontend(this IWebHostBuilder hostBuilder, Action<Configuration> configurationAction)
        {
            hostBuilder.Configure(app =>
            {
                var config = new Configuration(hostBuilder, app);
                configurationAction(config);

                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGroup($"/{config.BasePath}")
                       .MapFHIRRoutes();
                });
                
            });
            hostBuilder.ConfigureServices(services =>
            {
                //services.Configure<JsonSerializerOptions>(opt => opt.SetLumbreFHIRDefaults());
                services.Configure<JsonOptions>(opt =>
                {
                    opt.SerializerOptions.SetLumbreFHIRDefaults();
                });
            });
            return hostBuilder;
        }

        public static void UseSelfHosting(this Configuration configuration, Action<SelfHostedConfiguration>? configAction = default)
        {
            var config = new SelfHostedConfiguration();
            configAction?.Invoke(config);
            configuration.WebHost.UseKestrel(cfg => {
                cfg.Listen(new IPEndPoint(IPAddress.Parse(config.IP), config.Port));
            });
        }
    }
}
