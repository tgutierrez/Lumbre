using Hl7.Fhir.Model;
using Lumbre.Interfaces.Contracts;
using Lumbre.Middleware.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Middleware.Handlers.ForQueries
{
    internal class QueryByIdHandler : IRequestHandler<QueryByIdRequest, IOperationResponse>  
    {

        public Task<IOperationResponse> Handle(QueryByIdRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
