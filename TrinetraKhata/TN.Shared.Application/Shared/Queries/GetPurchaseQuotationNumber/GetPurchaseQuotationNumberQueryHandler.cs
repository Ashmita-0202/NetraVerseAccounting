using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetPurchaseQuotationNumber
{
    public class GetPurchaseQuotationNumberQueryHandler:IRequestHandler<GetPurchaseQuotationNumberQuery, Result<GetPurchaseQuotationNumberQueryResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public GetPurchaseQuotationNumberQueryHandler(ISettingServices settingServices,IMapper mapper)
        {
            _settingServices = settingServices;
            _mapper = mapper;
        }

        public async Task<Result<GetPurchaseQuotationNumberQueryResponse>> Handle(GetPurchaseQuotationNumberQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var purchaseQuotaionNumber = await _settingServices.GetPurchaseQuotationNumber(request.companyId, cancellationToken);
                var purchaseQuotaionNumberDisplay = _mapper.Map<GetPurchaseQuotationNumberQueryResponse>(purchaseQuotaionNumber.Data);
                return Result<GetPurchaseQuotationNumberQueryResponse>.Success(purchaseQuotaionNumberDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while Getting purchase Quotation number by company{request.companyId}");
            }
        }
    }
}
