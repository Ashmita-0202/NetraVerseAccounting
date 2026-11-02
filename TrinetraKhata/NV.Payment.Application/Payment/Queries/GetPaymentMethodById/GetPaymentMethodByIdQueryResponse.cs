using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NV.Payment.Domain.Entities.PaymentMethod;

namespace NV.Payment.Application.Payment.Queries.GetPaymentMethodById
{
    public record  GetPaymentMethodByIdQueryResponse
    (
        string id="",
        string name = "",
        string SubLedgerGroupsId = "",
            bool? isChequeNo=false,
            bool? isBankName = false,
            bool? isAccountName = false,
            bool? isChequeDate = false,
            bool? isCardNumber = false,
            bool? isCardHolderName = false,
            bool? isExpiryDate = false


    //PaymentType type = default
    );
}
