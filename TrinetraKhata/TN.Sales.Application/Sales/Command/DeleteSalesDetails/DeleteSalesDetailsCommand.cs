using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;


namespace TN.Sales.Application.Sales.Command.DeleteSalesDetails
{
    public record class DeleteSalesDetailsCommand
    (string id):IRequest<Result<bool>>;
}
