using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Utilities
{
    public static class SerializerOptions
    {
        public static JsonSerializerOptions Get() => new JsonSerializerOptions()
                    .SetLumbreFHIRDefaults();

        public static JsonSerializerOptions SetLumbreFHIRDefaults(this JsonSerializerOptions options)
        {
            options.ForFhir(ModelInfo.ModelInspector, new FhirJsonPocoDeserializerSettings
            {
                
            })
                    .Pretty();
            options.UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Skip;
            return options;
        }
    }
}
