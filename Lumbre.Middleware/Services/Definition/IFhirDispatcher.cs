using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Services.Definition
{
    public interface IFhirDispatcher
    {
        Task<IOperationResponse> Perform<T>(IFHIRRequest request) where T : IIdentifiable<List<Identifier>>, new();
    }
}
