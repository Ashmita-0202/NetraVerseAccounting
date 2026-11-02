using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetIncomeTransactionNumberType
{
    public  class GetIncomeTransactionNumberTypeQueryHandler:IRequestHandler<GetIncomeTransactionNumberTypeQuery,Result<GetIncomeTransactionNumberTypeResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public GetIncomeTransactionNumberTypeQueryHandler(ISettingServices settingServices,IMapper mapper)
        {
            _settingServices = settingServices;
            _mapper = mapper;

        }

        public async Task<Result<GetIncomeTransactionNumberTypeResponse>> Handle(GetIncomeTransactionNumberTypeQuery request, CancellationToken cancellationToken)
        {
            var incomeTransactionType = await _settingServices.GetIncomeTransactionType(request.companyId, cancellationToken);

            if (incomeTransactionType is not { IsSuccess: true, Data: not null })
                return Result<GetIncomeTransactionNumberTypeResponse>.Failure(incomeTransactionType?.Message ?? "Unable to TransactionNumberType reference.");

            return Result<GetIncomeTransactionNumberTypeResponse>.Success(
                _mapper.Map<GetIncomeTransactionNumberTypeResponse>(incomeTransactionType.Data)
            );
        }
    }
}
