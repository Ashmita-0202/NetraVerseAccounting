using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using TN.Shared.Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.UpdatePayment
{
    public record  UpdatePaymentCommand
    (
            string id = "",
            string transactionDate = "",
            decimal totalAmount = 0,
            string narration = "",
            TransactionType transactionMode = default,
            string? paymentsNumber = "",
            string? paymentMethodId = "",
            string? chequeNumber="",
            string? bankName="",
            string? accountName="",
            List<UpdateTransactionItemRequest> TransactionsItems = null!
    ) :IRequest<Result<UpdatePaymentResponse>>;
   
}
