using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Queries.GetReceiptTransactionNumberType;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetPaymentTransactionNumberType
{
    public class GetPaymentTransactionNumberTypeQueryHandler : IRequestHandler<GetPaymentTransactionNumberTypeQuery, Result<GetPaymentTransactionNumberTypeResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public GetPaymentTransactionNumberTypeQueryHandler(ISettingServices settingServices, IMapper mapper)
        {
            _settingServices = settingServices;
            _mapper = mapper;

        }
        public async Task<Result<GetPaymentTransactionNumberTypeResponse>> Handle(GetPaymentTransactionNumberTypeQuery request, CancellationToken cancellationToken)
        {
            var paymentTransactionType = await _settingServices.GetPaymentTransactionType(request.companyId, cancellationToken);

            if (paymentTransactionType is not { IsSuccess: true, Data: not null })
                return Result<GetPaymentTransactionNumberTypeResponse>.Failure(paymentTransactionType?.Message ?? "Unable to TransactionNumberType reference.");

            return Result<GetPaymentTransactionNumberTypeResponse>.Success(
                _mapper.Map<GetPaymentTransactionNumberTypeResponse>(paymentTransactionType.Data)
            );
        }
    }
}
