using Lumbre.Middleware.Services.Definition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumbre;
using Lumbre.Middleware;
using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Task = System.Threading.Tasks.Task;
using Testing.SimplePersistanceHarness;

namespace Testing.Integration.Query
{
    [TestClass]
    public class CanPerformQueryById
    {
        public readonly IFhirDispatcher _dispatcher;
        public readonly IServiceCollection _services;
        public readonly SimpleTestHarness _simpleDatabase;

        public CanPerformQueryById()
        {
            _services = new ServiceCollection()
                    .AddLumbre(cfg =>
                     {
                         cfg.AddSimpleTestHarnes();
                         //cfg.UseMongoDb(conn =>
                         //{
                         //    conn.DatabaseName = "FHIR";
                         //    conn.ConnectionString = "mongodb://localhost:27017/";
                         //});
                     });

            var builder = _services.BuildServiceProvider();

            _dispatcher = builder.GetService<IFhirDispatcher>();
        }

        [TestMethod]
        public async Task Get_Result_As_Object()
        {
            var result = await _dispatcher.GetObjectById<Patient>("1");

            Assert.IsNotNull(result);

            Assert.IsInstanceOfType<ValidResponse<ObjectResponse<Patient>>> (result);
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
            Assert.IsTrue(!String.IsNullOrEmpty(result.Response.Value));
        }

    }
}
