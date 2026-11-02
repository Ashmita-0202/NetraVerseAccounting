using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Queries.GetSalesReferenceNumber;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetPurchaseReferenceNumber
{
    public class GetPurchaseReferenceNumberQueryHandler : IRequestHandler<GetPurchaseReferenceNumberQuery, Result<GetPurchaseReferenceNumberQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISettingServices _settingServices;
        public GetPurchaseReferenceNumberQueryHandler(ISettingServices settingServices, IMapper mapper)
        {
            _mapper = mapper;
            _settingServices = settingServices;
        }
        public async Task<Result<GetPurchaseReferenceNumberQueryResponse>> Handle(GetPurchaseReferenceNumberQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var purchaseReferenceNumber = await _settingServices.GetPurchaseReferenceNumber(request.companyId, cancellationToken);
                var purchaseReferenceNumberDisplay = _mapper.Map<GetPurchaseReferenceNumberQueryResponse>(purchaseReferenceNumber.Data);
                return Result<GetPurchaseReferenceNumberQueryResponse>.Success(purchaseReferenceNumberDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while Getting purchase reference number by company{request.companyId}");
            }
        }
    }
}
