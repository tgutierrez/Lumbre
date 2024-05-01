using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Lumbre.Interfaces.Contracts;
using Lumbre.Interfaces.Repository;
using Lumbre.Middleware.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Handlers.ForCommands
{
    internal class DeleteRequestCommandHandler<T> : IRequestHandler<DeleteRequestCommand<T>, IResponse<Outcome>> where T : IIdentifiable<List<Identifier>>, new()
    {
        private readonly IRepository _repository;

        public DeleteRequestCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResponse<Outcome>> Handle(DeleteRequestCommand<T> request, CancellationToken cancellationToken)
        {
            var collectionName = new Interfaces.Common.Primitives.CollectionName((request.RequestContent.Entity as DomainResource).TypeName);
            return await _repository.DeleteById(collectionName, request.RequestContent.Id) switch
            {
                NotFound => new Rejected([$"Resource id: {request.RequestContent.Id} from:{collectionName} not found"]),
                Completed c => new AcceptedResponse<Completed>(c)
            };
}
    }
}
