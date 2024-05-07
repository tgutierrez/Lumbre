using Hl7.Fhir.Model;
using Lumbre.Interfaces.Query.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Interfaces.Query.Definitions
{
    public class PatientQuery : IQuery<Patient>
    {
        [JsonPath("birthDay", ExpressionType.Equal)]
        public DateTime BirthDay { get; } // Todo, Add parameters

        [JsonPath("id", ExpressionType.Equal)]
        public string Id { get; }

        [JsonPath("managingOrganization.reference", ExpressionType.Equal)]
        public string Organization { get; }

        [JsonPath("meta.lastUpdated", 
            ExpressionType.Equal, 
            ExpressionType.GreaterThan, 
            ExpressionType.GreaterThanOrEqual, 
            ExpressionType.LessThan, 
            ExpressionType.LessThanOrEqual)]
        public DateTime LastUpdated { get; }
    }

}
