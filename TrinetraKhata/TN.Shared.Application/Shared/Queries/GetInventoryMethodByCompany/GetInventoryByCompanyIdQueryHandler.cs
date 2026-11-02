using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Queries.GetJournalRefByCompany;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetInventoryMethodByCompany
{
    public  class GetInventoryByCompanyIdQueryHandler:IRequestHandler<GetInventoryByCompanyIdQuery, Result<GetInventoryByCompanyIdQueryResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public GetInventoryByCompanyIdQueryHandler(ISettingServices settingServices, IMapper mapper)
        {
            _settingServices = settingServices;
            _mapper = mapper;
        }

        public async Task<Result<GetInventoryByCompanyIdQueryResponse>> Handle(GetInventoryByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var inventoryMethod = await _settingServices.GetInventoryMethodByCompany(request.companyId, cancellationToken);
                var inventoryMethodDisplay = _mapper.Map<GetInventoryByCompanyIdQueryResponse>(inventoryMethod.Data);

                return Result<GetInventoryByCompanyIdQueryResponse>.Success(inventoryMethodDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while Getting inventory method by company {request.companyId}");
            }
        }
    }
}
