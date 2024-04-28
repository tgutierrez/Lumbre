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

namespace Testing.Integration.FhirDispatcher.InMemoryPersistance
{
    [TestClass]
    public class CanInsertSingleResult
    {
        public readonly IFhirDispatcher _dispatcher;
        public readonly IServiceCollection _services;
        public readonly InMemoryPersistanceTestHarness _simpleDatabase;

        public CanInsertSingleResult()
        {
            InMemoryPersistanceTestHarness.Initialize();

            _services = new ServiceCollection()
                    .AddLumbre(cfg =>
                    {
                        cfg.UseInMemoryPersistanceTestHarness();
                    });

            var builder = _services.BuildServiceProvider();

            _dispatcher = builder.GetService<IFhirDispatcher>();
            _simpleDatabase = builder.GetService<InMemoryPersistanceTestHarness>();
        }

        [TestMethod]
        public async Task InsertObject()
        {
            var practitioner = new Practitioner()
            {
                Id = "1",
                Name = { new HumanName() { Family = "Riviera", Given = ["Nick"] } }
            };

            var response = await _dispatcher.PutObject(practitioner, "1");

            Assert.IsInstanceOfType<AcceptedResponse<Practitioner>>(response);

            Assert.AreEqual(1, _simpleDatabase.Data[new Primitives.CollectionName("Practitioner")].Count());
        }

        [TestMethod]
        public async Task InsertObjectWithoutId()
        {
            var practitioner = new Practitioner()
            {
                Name = { new HumanName() { Family = "Riviera", Given = ["Nick"] } }
            };

            var response = await _dispatcher.PutObject(practitioner, "1");

            Assert.IsInstanceOfType<Rejected>(response);
            Assert.AreEqual(2, ((Rejected)response).Reasons.Length);
            Assert.AreEqual("Id missing from Resource", ((Rejected)response).Reasons[0]);
            Assert.AreEqual("Insertion Id and Resource Id must be equal", ((Rejected)response).Reasons[1]);
        }
    }
}
