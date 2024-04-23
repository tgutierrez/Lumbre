using Lumbre.Interfaces.Contracts;
using Lumbre.Interfaces.Repository;
using Lumbre.Persistance.Mongodb.Configuration;
using Lumbre.Persistance.Mongodb.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre
{
    public static class Extensions
    {
        public static IConfigurator UseMongoDb(this IConfigurator configurator, Action<MongoDBConfigurator> configure)
        {
            var dbConfigurator = new MongoDBConfigurator();
            configure?.Invoke(dbConfigurator);
            configurator.Services.AddSingleton(dbConfigurator.GetConfig());
            configurator.Services.AddScoped<IRepository, MongoRepository>();

            return configurator;
        }

    }
}
