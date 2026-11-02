using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NV.Payment.Domain.Entities.PaymentMethod;

namespace NV.Payment.Application.Payment.Command.UpdatePayment
{
    public record  UpdatePaymentMethodRequest
    (
            string name,
             string subLedgerGroupsId
            //PaymentType type

    );
}
