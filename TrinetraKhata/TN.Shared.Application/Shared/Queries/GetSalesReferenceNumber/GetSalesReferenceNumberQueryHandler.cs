using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Queries.GetSerialNumberForPurchase;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetSalesReferenceNumber
{
    public class GetSalesReferenceNumberQueryHandler : IRequestHandler<GetSalesReferenceNumberQuery, Result<GetSalesReferenceNumberQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISettingServices _settingServices;

        public GetSalesReferenceNumberQueryHandler(ISettingServices settingServices, IMapper mapper)
        {
            _mapper = mapper;
            _settingServices = settingServices;
        }
        public async Task<Result<GetSalesReferenceNumberQueryResponse>> Handle(GetSalesReferenceNumberQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var salesReferenceNumber = await _settingServices.GetSalesReferenceNumber(request.companyId, cancellationToken);
                var salesReferenceNumberDisplay = _mapper.Map<GetSalesReferenceNumberQueryResponse>(salesReferenceNumber.Data);
                return Result<GetSalesReferenceNumberQueryResponse>.Success(salesReferenceNumberDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while Getting serial number by company{request.companyId}");
            }
        }
    }
}
