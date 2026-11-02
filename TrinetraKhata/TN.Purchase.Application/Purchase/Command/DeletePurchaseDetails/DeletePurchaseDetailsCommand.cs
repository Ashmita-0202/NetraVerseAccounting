using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Command.DeletePurchaseDetails
{
    public  record DeletePurchaseDetailsCommand
   (string id):IRequest<Result<bool>>;
}
