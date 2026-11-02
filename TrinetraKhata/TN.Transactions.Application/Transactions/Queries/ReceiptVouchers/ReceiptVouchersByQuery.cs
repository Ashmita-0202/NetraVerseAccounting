using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Transactions.Application.Transactions.Queries.ReceiptVouchers
{
    public record ReceiptVouchersByQuery
    ( string transactionsDetailsId
        ) : IRequest<Result<ReceiptVouchersByQueryResponse>>;
    
}
