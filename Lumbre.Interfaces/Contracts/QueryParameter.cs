using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Interfaces.Contracts
{
    public class Query<T> where T : IIdentifiable<List<Identifier>>, new()
    {
        public string Id { get; set; }
    }
}
