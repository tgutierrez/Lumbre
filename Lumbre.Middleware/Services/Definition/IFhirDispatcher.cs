using Hl7.Fhir.Model;
using Lumbre.Interfaces.Common;
using Lumbre.Interfaces.Contracts;
using Lumbre.Interfaces.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Services.Definition
{
    public interface IFhirDispatcher
    {
        Task<IResponse<K>> Perform<T, K>(IFHIRRequest request) where T : IIdentifiable<List<Identifier>>, new() where K : IExpectedResponseType;

        Task<IResponse<K>> PerformQuery<T, K, Q>(IFHIRRequest request) where T : IIdentifiable<List<Identifier>>, new() where K : IExpectedResponseType where Q : IQuery<T>;
    }
}
