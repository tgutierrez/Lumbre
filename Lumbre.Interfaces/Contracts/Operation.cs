using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Interfaces.Contracts
{
    public interface IOperation<out TRequest, out TResponseType> where TRequest: IFHIRRequest where TResponseType : IExpectedResponseType
    {
        TRequest Request { get; }
        TResponseType Response { get; }
    }
}
