using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lumbre.Interfaces.Common.Primitives;

namespace Lumbre.Interfaces.Contracts
{
    public record Operation<TRequest, TResponseType>(TRequest Request, TResponseType Response) : IOperation<TRequest, TResponseType> where TRequest : IFHIRRequest where TResponseType : IExpectedResponseType;

    public record QueryById<T>(ResourceId Id) : IFHIRRequest where T : IIdentifiable<List<Identifier>>, new()
    {
        public IIdentifiable<List<Identifier>> Entity => new T(); 
    }

    public record InsertRecord<T>(T Resource) : IFHIRRequest where T : IIdentifiable<List<Identifier>>, new()
    {
        public IIdentifiable<List<Identifier>> Entity => throw new NotImplementedException();
    }

    public record ValidResponse<T>(string Message, T Response, JsonPayload serializedResponse) : IResponse<T> where T: IExpectedResponseType
    {
        public bool IsSuccess => true;
    }

    public record ResourceNotFound<T>(string collection, ResourceId id) : IResponse<T> where T : IExpectedResponseType
    {
        public bool IsSuccess => false;

        public T? Response => default(T);

        public string Message => $"{collection}: Resource not found (id: {id})";
    }

    public record ErrorResponse<T>(string Message, string[] ErrorList):  IResponse<T> where T : IExpectedResponseType
    { 
        public bool IsSuccess => false;

        public T? Response => default(T);
    }

    public record ObjectResponse<T>(T Value) : IExpectedResponseType where T : IIdentifiable<List<Identifier>>, new()
    {
        public string Name() => typeof(T).Name;
    }

    public record JsonResponse(JsonPayload Value) : IExpectedResponseType
    {
        public string Name() => "Json Value";
    }
}
