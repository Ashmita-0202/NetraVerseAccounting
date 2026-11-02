using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Queries.GetJournalRefByCompany;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetReceiptTransactionNumberType
{
    public class ReceiptTransactionNumberTypeQueryHandler : IRequestHandler<ReceiptTransactionNumberTypeQuery, Result<ReceiptTransactionNumberTypeResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public ReceiptTransactionNumberTypeQueryHandler(ISettingServices settingServices, IMapper mapper)
        {
            _settingServices = settingServices;
            _mapper = mapper;

        }
        public async Task<Result<ReceiptTransactionNumberTypeResponse>> Handle(ReceiptTransactionNumberTypeQuery request, CancellationToken cancellationToken)
        {
            var receiptTransactionType = await _settingServices.GetReceiptTransactionType(request.companyId, cancellationToken);

            if (receiptTransactionType is not { IsSuccess: true, Data: not null })
                return Result<ReceiptTransactionNumberTypeResponse>.Failure(receiptTransactionType?.Message ?? "Unable to TransactionNumberType reference.");

            return Result<ReceiptTransactionNumberTypeResponse>.Success(
                _mapper.Map<ReceiptTransactionNumberTypeResponse>(receiptTransactionType.Data)
            );
        }
    }
}
