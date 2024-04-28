using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Middleware.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Behaviors
{
    internal class TagMeta<T> : IPipelineBehavior<PutRequestCommand<T>, IResponse<Outcome>> where T : IIdentifiable<List<Identifier>>, new()
    {
        public async Task<IResponse<Outcome>> Handle(PutRequestCommand<T> request, RequestHandlerDelegate<IResponse<Outcome>> next, CancellationToken cancellationToken)
        {
            (request.RequestContent.Resource as DomainResource).Meta = new Meta
            {
                VersionId = Guid.NewGuid().ToString(),
                LastUpdated = DateTime.UtcNow,
            };

            return await next();
        }
    }
}
