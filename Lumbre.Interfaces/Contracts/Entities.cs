using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lumbre.Interfaces.Common.Primitives;

namespace Lumbre.Interfaces.Contracts
{
    public record QueryById<T>(ResourceId Id) : IFHIRRequest where T : IIdentifiable<List<Identifier>>, new()
    {
        public IIdentifiable<List<Identifier>> Entity => new T(); 
    }

    public record InsertRecord<T>(T Resource) : IFHIRRequest where T : IIdentifiable<List<Identifier>>
    {
        public IIdentifiable<List<Identifier>> Entity => throw new NotImplementedException();
    }

    public record ValidResponse<T>(string Message, T Response, JsonPayload serializedResponse) : IOperationResponse where T: DocumentReference
    {
        public bool IsSuccess => true;
    }

    public record ErrorResponse(string Message, string[] ErrorList): IOperationResponse { 
        public bool IsSuccess => false;
    }
}
