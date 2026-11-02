using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetTaxStatusInPurchase
{
    public class GetTaxStatusInPurchaseQueryHandler : IRequestHandler<GetTaxStatusInPurchaseQuery, Result<GetTaxStatusInPurchaseResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISettingServices _settingServices;

        public GetTaxStatusInPurchaseQueryHandler(ISettingServices settingServices, IMapper mapper)
        {
            _mapper = mapper;
            _settingServices = settingServices;
        }
        public async Task<Result<GetTaxStatusInPurchaseResponse>> Handle(GetTaxStatusInPurchaseQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var taxStatusInPurchase = await _settingServices.GetTaxStatusInPurchase(request.companyId, cancellationToken);
                var taxStatusInPurchaseDetails = _mapper.Map<GetTaxStatusInPurchaseResponse>(taxStatusInPurchase.Data);
                return Result<GetTaxStatusInPurchaseResponse>.Success(taxStatusInPurchaseDetails);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while Getting tax status for Purchase by company{request.companyId}");
            }
        }
    }
}
