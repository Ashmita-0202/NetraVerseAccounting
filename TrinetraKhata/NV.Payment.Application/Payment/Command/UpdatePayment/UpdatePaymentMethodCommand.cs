using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using static NV.Payment.Domain.Entities.PaymentMethod;

namespace NV.Payment.Application.Payment.Command.UpdatePayment
{
    public record  UpdatePaymentMethodCommand
    (
            string id,
             string name,
            string subLedgerGroupsId
            //PaymentType type
    ) :IRequest<Result<UpdatePaymentMethodResponse>>;
}
