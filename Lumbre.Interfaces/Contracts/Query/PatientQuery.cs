using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Interfaces.Contracts.Query
{
    public class PatientQuery : Query<Patient> 
    {
        public Date BirthDay { get; set; } // Todo, Add parameters

        public String Id { get; set; }

        public String Organization { get; set; }
    }
}
