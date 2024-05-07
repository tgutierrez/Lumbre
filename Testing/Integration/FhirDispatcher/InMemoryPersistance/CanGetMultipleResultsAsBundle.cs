using Hl7.Fhir.Model;
using Lumbre;
using Lumbre.Interfaces.Contracts;
using Lumbre.Interfaces.Query;
using Lumbre.Interfaces.Query.Definitions;
using Lumbre.Middleware;
using Lumbre.Middleware.Services.Definition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.InMemoryPersistance;
using static Lumbre.Interfaces.Common.Primitives;

namespace Testing.Integration.FhirDispatcher.InMemoryPersistance
{
    [TestClass]
    public class CanGetMultipleResultsAsBundle
    {
        public readonly IFhirDispatcher _dispatcher;
        public readonly IServiceCollection _services;
        public readonly InMemoryPersistanceTestHarness _simpleDatabase;

        public CanGetMultipleResultsAsBundle()
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
        public async System.Threading.Tasks.Task CanGetAResultAsBundle()
        {
            var outcome = await _dispatcher.GetResultsFor<Patient, PatientQuery>(p => p.Organization == "Lumbre");
            Assert.IsNotNull(outcome);
            Assert.IsInstanceOfType<BundledRespose>(outcome);

        }
    }
}
