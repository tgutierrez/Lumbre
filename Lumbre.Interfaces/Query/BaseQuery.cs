using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Interfaces.Query
{
    public interface IQuery<out T> where T : IIdentifiable<List<Identifier>>, new()
    {
    }
}
