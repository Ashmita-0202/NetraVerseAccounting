using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;
using TN.Transactions.Application.Transactions.Queries.GetAllTransactions;
using TN.Transactions.Application.Transactions.Queries.GetTransactionsById;

namespace TN.Transactions.Application.Transactions.Queries.ReceiptVouchers
{
    public class ReceiptVouchersByQueryHandler : IRequestHandler<ReceiptVouchersByQuery, Result<ReceiptVouchersByQueryResponse>>
    {

        private readonly ITransactionsService _service;
        private readonly IMapper _mapper;

        public ReceiptVouchersByQueryHandler(ITransactionsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<ReceiptVouchersByQueryResponse>> Handle(ReceiptVouchersByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var voucherReceipt = await _service.GetReceiptVouchers(request.transactionsDetailsId);
                return Result<ReceiptVouchersByQueryResponse>.Success(voucherReceipt.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Transactions by using id", ex);
            }
        }
    }
}
