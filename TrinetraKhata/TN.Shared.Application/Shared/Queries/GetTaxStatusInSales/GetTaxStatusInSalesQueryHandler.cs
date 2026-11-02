using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetTaxStatusInSales
{
    public class GetTaxStatusInSalesQueryHandler : IRequestHandler<GetTaxStatusInSalesQuery, Result<GetTaxStatusInSalesResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISettingServices _settingServices;
        public GetTaxStatusInSalesQueryHandler(ISettingServices settingServices, IMapper mapper)
        {
            _mapper = mapper;
            _settingServices = settingServices;
        }
        public async Task<Result<GetTaxStatusInSalesResponse>> Handle(GetTaxStatusInSalesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var taxStatusInSales = await _settingServices.GetTaxStatusInSales(request.companyId, cancellationToken);
                var taxStatusInSalesDetails = _mapper.Map<GetTaxStatusInSalesResponse>(taxStatusInSales.Data);
                return Result<GetTaxStatusInSalesResponse>.Success(taxStatusInSalesDetails);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while Getting tax status for Purchase by company{request.companyId}");
            }
        }
    }
}
