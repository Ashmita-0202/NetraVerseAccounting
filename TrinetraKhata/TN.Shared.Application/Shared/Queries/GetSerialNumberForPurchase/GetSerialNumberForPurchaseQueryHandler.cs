using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetSerialNumberForPurchase
{
    public class GetSerialNumberForPurchaseQueryHandler:IRequestHandler<GetSerialNumberForPurchaseQuery, Result<GetSerialNumberForPurchaseQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISettingServices _settingServices;

        public GetSerialNumberForPurchaseQueryHandler(ISettingServices settingServices, IMapper mapper) 
        {
            _mapper = mapper;
            _settingServices = settingServices;
        }

        public async Task<Result<GetSerialNumberForPurchaseQueryResponse>> Handle(GetSerialNumberForPurchaseQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var serialNumberForPurchase = await _settingServices.GetItemStatusByCompany(request.companyId, cancellationToken);
                var serialNumberForPurchaseDisplay = _mapper.Map<GetSerialNumberForPurchaseQueryResponse>(serialNumberForPurchase.Data);
                return Result<GetSerialNumberForPurchaseQueryResponse>.Success(serialNumberForPurchaseDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while Getting serial number by company{request.companyId}");
            }
        }
    }
}
