using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Interfaces.Repository;
using Lumbre.Middleware.Requests;
using Lumbre.Interfaces.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Handlers.ForCommands
{
    internal class PutRequestCommandHandler<T> : IRequestHandler<PutRequestCommand<T>, IResponse<Outcome>> where T : IIdentifiable<List<Identifier>>, new()
    {
        private readonly IRepository _repository;
        public PutRequestCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResponse<Outcome>> Handle(PutRequestCommand<T> request, CancellationToken cancellationToken)
        {
            var insertable = request.RequestContent.Resource as DomainResource;
            // add meta
            insertable.Meta = new Meta
            {
                LastUpdated = DateTime.UtcNow,
                VersionId = Guid.NewGuid().ToString(),
            };
            await _repository.Upsert(
                new Primitives.CollectionName(insertable.TypeName),
                new Primitives.ResourceId(request.RequestContent.Id),
                request.JsonPayload.Value);

            return new AcceptedResponse();
        }
    }
}
