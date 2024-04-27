using Lumbre.Middleware.Services.Definition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lumbre;
using Lumbre.Middleware;
using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Task = System.Threading.Tasks.Task;
using Testing.InMemoryPersistance;

namespace Testing.Integration.FhirDispatcher.MongoDB
{
    [TestClass]
    public class CanPerformQueryById
    {
        public readonly IFhirDispatcher _dispatcher;
        public readonly IServiceCollection _services;
        public readonly InMemoryPersistanceTestHarness _simpleDatabase;
        public readonly string ConnString = "mongodb://localhost:27017/"; // I know, this should go in a config file...

        public CanPerformQueryById()
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
        public async Task Get_Result_As_Object()
        {
            var result = await _dispatcher.GetObjectById<Patient>("1");

            Assert.IsNotNull(result);

            Assert.IsInstanceOfType<ValidResponse<ObjectResponse<Patient>>>(result);
            Assert.IsNotNull(result.Response.Value);
            Assert.AreEqual("1", result.Response.Value.Id);
        }

        [TestMethod]
        public async Task Get_Result_As_JsonString()
        {
            var result = await _dispatcher.GetJsonById<Patient>("1");

            Assert.IsNotNull(result);

            Assert.IsInstanceOfType<ValidResponse<JsonResponse>>(result);
            Assert.IsNotNull(result.Response.Value);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Response.Value));
        }

    }
}
