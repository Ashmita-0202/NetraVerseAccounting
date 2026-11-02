using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NV.Payment.Domain.Entities.PaymentMethod;

namespace NV.Payment.Application.Payment.Command.AddPayment
{
    public record  AddPaymentMethodResponse
    (
            string id="",
            string name="",
           string subLedgerGroupsId=""
            //PaymentType type= default
    );
}
