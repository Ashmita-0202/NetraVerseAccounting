using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.PurchaseReturn.Command.DeletePurchaseReturnDetails
{
    public record  DeletePurchaseReturnDetailsCommand
    ( string id):IRequest<Result<bool>>;
}
