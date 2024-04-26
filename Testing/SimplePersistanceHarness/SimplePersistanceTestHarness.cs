using Amazon.Runtime.Internal.Transform;
using Lumbre.Interfaces.Common;
using Lumbre.Interfaces.Contracts;
using Lumbre.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lumbre.Interfaces.Common.Primitives;

namespace Testing.SimplePersistanceHarness
{
    /// <summary>
    /// Simple test harness 
    /// </summary>
    public class SimplePersistanceTestHarness : IRepository
    {
        private static readonly Dictionary<CollectionName, Dictionary<ResourceId, JsonPayload>> _data = new Dictionary<CollectionName, Dictionary<ResourceId, JsonPayload>>();
        


        public static void Initialize()
        {
            _data.Clear();
            _data.Add(new CollectionName("Patient"), new Dictionary<ResourceId, JsonPayload>());
            var jsonInput = File.ReadAllText("SimplePersistanceHarness\\BaseTestData\\Patient.json");
            _data[new CollectionName("Patient")].Add(new ResourceId("1"), new JsonPayload(jsonInput));
        }

        public Dictionary<CollectionName, Dictionary<ResourceId, JsonPayload>> Data => _data;


        public Task<IRepositoryResult> ReadById(Primitives.CollectionName collectionName, Primitives.ResourceId resourceId)
        {
            if (_data[collectionName].TryGetValue(resourceId, out var result))
            {
                return Task.FromResult(new Results(result) as IRepositoryResult);
            }

            return Task.FromResult(new NotFound() as IRepositoryResult);  
        }
            

        public Task Upsert(Primitives.CollectionName collection, Primitives.ResourceId resourceId, Primitives.JsonPayload payload)
        {
            if (!_data.ContainsKey(collection))
            {
                _data.Add(collection, new Dictionary<ResourceId, JsonPayload>());
            }

            _data[collection].Add(resourceId, payload);

            return Task.CompletedTask;
        }
    }

    public static class Extension
    {
        public static IConfigurator AddSimpleTestHarnes(this IConfigurator cfg)
        {
            cfg.Services.AddTransient<IRepository, SimplePersistanceTestHarness>();
            cfg.Services.AddTransient<SimplePersistanceTestHarness>(); // Register itself so we can access it's properties
            return cfg;
        }
    }
}
