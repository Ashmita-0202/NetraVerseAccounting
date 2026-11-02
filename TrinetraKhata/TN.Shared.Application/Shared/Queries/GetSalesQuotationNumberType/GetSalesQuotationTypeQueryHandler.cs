using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetSalesQuotationNumberType
{
    public class GetSalesQuotationTypeQueryHandler:IRequestHandler<GetSalesQuotationTypeQuery,Result<GetSalesQuotationTypeQueryResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public GetSalesQuotationTypeQueryHandler(ISettingServices settingServices,IMapper mapper)
        {
            _settingServices = settingServices;
            _mapper = mapper;
        }

        public async Task<Result<GetSalesQuotationTypeQueryResponse>> Handle(GetSalesQuotationTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var salesQuotaionNumber = await _settingServices.GetSalesQuotationType(request.companyId, cancellationToken);
                var salesQuotaionNumberDisplay = _mapper.Map<GetSalesQuotationTypeQueryResponse>(salesQuotaionNumber.Data);
                return Result<GetSalesQuotationTypeQueryResponse>.Success(salesQuotaionNumberDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while Getting sales Quotation number by company{request.companyId}");
            }
        }
    }
}
