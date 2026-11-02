using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetPurchaseReturnNumber
{
    public  class GetPurchaseReturnNumberQueryHandler:IRequestHandler<GetPurchaseReturnNumberQuery,Result<GetPurchaseReturnNumberQueryResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public GetPurchaseReturnNumberQueryHandler(ISettingServices settingServices,IMapper mapper) 
        {
            _settingServices = settingServices;
            _mapper = mapper;
        }

        public async Task<Result<GetPurchaseReturnNumberQueryResponse>> Handle(GetPurchaseReturnNumberQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var returnNumber = await _settingServices.GetPurchaseReturnNumber(request.companyId, cancellationToken);
                var returnNumberDisplay = _mapper.Map<GetPurchaseReturnNumberQueryResponse>(returnNumber.Data);
                return Result<GetPurchaseReturnNumberQueryResponse>.Success(returnNumberDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while Getting purchase return number by company{request.companyId}");
            }

        }
    }
}
