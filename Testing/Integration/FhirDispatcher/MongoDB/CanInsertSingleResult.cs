using Lumbre.Middleware.Services.Definition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lumbre;
using Lumbre.Middleware;
using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Interfaces.Common;
using Task = System.Threading.Tasks.Task;
using Testing.InMemoryPersistance;
using Microsoft.Extensions.Options;
using System.Text.Json;
using MongoDB.Driver.Core.Configuration;

namespace Testing.Integration.FhirDispatcher.MongoDB
{
    [TestClass]
    public class CanInsertSingleResult
    {
        public readonly IFhirDispatcher _dispatcher;
        public readonly IServiceCollection _services;
        public readonly string ConnString = "mongodb://localhost:27017/"; // I know, this should go in a config file...
        public CanInsertSingleResult()
        {
            var conn = "mongodb://localhost:27017/";
            MongoDBTestHelper.Initialize(conn);

            _services = new ServiceCollection()
                    .AddLumbre(cfg =>
                    {
                        cfg.UseMongoDb(cfg =>
                        {
                            cfg.ConnectionString = conn;
                            cfg.DatabaseName = MongoDBTestHelper.DbName;
                        });
                    });

            var builder = _services.BuildServiceProvider();

            _dispatcher = builder.GetService<IFhirDispatcher>();
        }

        [TestMethod]
        public async Task InsertObject()
        {
            var practitioner = new Practitioner()
            {
                Id = "2",
                Name = { new HumanName() { Family = "Riviera", Given = ["Nick"] } }
            };

            var response = await _dispatcher.PutObject(practitioner, "2");

            Assert.IsInstanceOfType<AcceptedResponse>(response);

            Assert.That.ExistOnDb(ConnString, "Practitioner", f => f.Eq("id", "2"));
        }

    }
}
