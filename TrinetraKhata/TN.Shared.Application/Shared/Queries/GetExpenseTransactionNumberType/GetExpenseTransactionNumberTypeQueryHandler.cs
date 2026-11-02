using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Queries.GetReceiptTransactionNumberType;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetExpenseTransactionNumberType
{
    public  class GetExpenseTransactionNumberTypeQueryHandler: IRequestHandler<GetExpenseTransactionNumberTypeQuery, Result<GetExpenseTransactionNumberTypeResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public GetExpenseTransactionNumberTypeQueryHandler(ISettingServices settingServices,IMapper mapper)
        {
            _settingServices = settingServices;
            _mapper = mapper;
        }

        public async Task<Result<GetExpenseTransactionNumberTypeResponse>> Handle(GetExpenseTransactionNumberTypeQuery request, CancellationToken cancellationToken)
        {
            var expenseTransactionType = await _settingServices.GetExpenseTransactionType(request.companyId, cancellationToken);

            if (expenseTransactionType is not { IsSuccess: true, Data: not null })
                return Result<GetExpenseTransactionNumberTypeResponse>.Failure(expenseTransactionType?.Message ?? "Unable to TransactionNumberType reference.");

            return Result<GetExpenseTransactionNumberTypeResponse>.Success(
                _mapper.Map<GetExpenseTransactionNumberTypeResponse>(expenseTransactionType.Data)
            );
        }
    }
}
