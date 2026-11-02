using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.DeleteCustomer
{
    public record DeleteCustomerCommand(string id):IRequest<Result<bool>>;
    
}
