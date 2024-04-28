using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Middleware.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Validation;
using System.Collections.ObjectModel;

namespace Lumbre.Middleware.Behaviors
{
    public class ValidateId<T, K> : IPipelineBehavior<QueryByIdRequest<T, K>, IResponse<K>> where T : IIdentifiable<List<Identifier>>, new()
                                                                                            where K : IExpectedResponseType
    {
        async Task<IResponse<K>> IPipelineBehavior<QueryByIdRequest<T, K>, IResponse<K>>.Handle(QueryByIdRequest<T, K> request, RequestHandlerDelegate<IResponse<K>> next, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(request.RequestContent.Id))
            {
                return new ErrorResponse<K>($"id for {typeof(T).Name} must be not null or empty", [""]);
            }

            return await next();
        }
    }

    public class ValidateObject<T> : IPipelineBehavior<PutRequestCommand<T>, IResponse<Outcome>> where T : IIdentifiable<List<Identifier>>, new()
    {
        async Task<IResponse<Outcome>> IPipelineBehavior<PutRequestCommand<T>, IResponse<Outcome>>.Handle(PutRequestCommand<T> request, RequestHandlerDelegate<IResponse<Outcome>> next, CancellationToken cancellationToken)
        {
            var results = new Collection<ValidationResult>();
            var baseelement = request.RequestContent.Resource as DomainResource;
            baseelement.TryValidate(results, true, NarrativeValidationKind.FhirXhtml);
            ValidateIdContent(request.RequestContent, baseelement, results);
            if ((results?.Count() ?? 0) > 0)
            {
                return new Rejected(results.Select(v => v.ErrorMessage).ToArray());
            }          

            return await next();   
        }

        private void ValidateIdContent(PutRequest<T> requestContent, DomainResource baseelement, Collection<ValidationResult> results)
        {
            if (String.IsNullOrEmpty(baseelement.Id)) 
            {
                results.Add(new ValidationResult("Id missing from Resource"));
            }

            if (baseelement.Id != requestContent.Id)
            {
                results.Add(new ValidationResult("Insertion Id and Resource Id must be equal"));
            }
        }
    }
}
