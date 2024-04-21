using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Interfaces.Contracts
{
    public static class Limits
    {
        /// <summary>
        /// Get all the currently supported FHIR types
        /// </summary>
        /// <returns></returns>
        public static Type[] GetSupportedDocumentTypes() => [typeof(Patient), typeof(Practitioner), typeof(Location), typeof(Appointment), typeof(PractitionerRole), typeof(Procedure)]; // todo: more types

        /// <summary>
        /// Get all the currently supported Return Types
        /// </summary>
        /// <returns></returns>
        public static Type[] GetSupportedReturnTypes() => typeof(Limits)
                                                        .Assembly
                                                        .GetTypes()
                                                        .Where(t => typeof(IExpectedResponseType).IsAssignableFrom(t) )
                                                        .ToArray();         
    }
}
