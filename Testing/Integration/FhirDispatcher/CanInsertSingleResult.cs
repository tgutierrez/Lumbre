using Lumbre.Middleware.Services.Definition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lumbre;
using Lumbre.Middleware;
using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Interfaces.Common;
using Task = System.Threading.Tasks.Task;
using Testing.SimplePersistanceHarness;

namespace Testing.Integration.FhirDispatcher
{
    [TestClass]
    public class CanInsertSingleResult
    {
        public readonly IFhirDispatcher _dispatcher;
        public readonly IServiceCollection _services;
        public readonly SimplePersistanceTestHarness _simpleDatabase;

        public CanInsertSingleResult()
        {
            SimplePersistanceTestHarness.Initialize();

            _services = new ServiceCollection()
                    .AddLumbre(cfg =>
                    {
                        cfg.AddSimpleTestHarnes();
                    });

            var builder = _services.BuildServiceProvider();

            _dispatcher = builder.GetService<IFhirDispatcher>();
            _simpleDatabase = builder.GetService<SimplePersistanceTestHarness>();
        }

        [TestMethod]
        public async Task SaveWithNoIssues()
        {
            var practitioner = new Practitioner()
            {
                Id = "1",
                Name = { new HumanName() { Family = "Riviera", Given = ["Nick"] } }
            };

            var response = await _dispatcher.PutObject(practitioner);

            Assert.IsInstanceOfType<AcceptedResponse>(response);

            Assert.AreEqual(1, _simpleDatabase.Data[new Primitives.CollectionName("Practitioner")].Count());
        } 
    }
}
