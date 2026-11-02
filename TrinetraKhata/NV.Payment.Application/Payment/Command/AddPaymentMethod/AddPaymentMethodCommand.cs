using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using static NV.Payment.Domain.Entities.PaymentMethod;

namespace NV.Payment.Application.Payment.Command.AddPayment
{
    public record AddPaymentMethodCommand
    (
            string name,
           string subLedgerGroupsId,
            //PaymentType type,
            bool? isChequeNo,
            bool? isBankName,
            bool? isAccountName,
            bool? isChequeDate,
            bool? isCardNumber,
            bool? isCardHolderName,
            bool? isExpiryDate


    ) :IRequest<Result<AddPaymentMethodResponse>>;
}
