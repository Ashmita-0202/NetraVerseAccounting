using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NV.Payment.Domain.Entities.PaymentMethod;

namespace NV.Payment.Application.Payment.Queries.GetPaymentMethod
{
    public record  GetAllPaymentMethodQueryResponse
    (
            string id ="",
            string name = "",
            string subLedgerGroupsId = "",
              bool? isChequeNo=false,
            bool? isBankName = false,
            bool? isAccountName = false,
            bool? isChequeDate = false,
            bool? isCardNumber = false,
            bool? isCardHolderName = false,
            bool? isExpiryDate = false


    );
}
