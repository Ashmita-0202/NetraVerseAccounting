using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetSalesReturnNumber
{
    public class GetSalesReturnNumberQueryHandler: IRequestHandler<GetSalesReturnNumberQuery, Result<GetSalesReturnNumberQueryResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public GetSalesReturnNumberQueryHandler(ISettingServices settingServices,IMapper mapper)
        {
            _settingServices= settingServices;
            _mapper = mapper;
        }

        public async Task<Result<GetSalesReturnNumberQueryResponse>> Handle(GetSalesReturnNumberQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var returnNumber = await _settingServices.GetSalesReturnNumber(request.companyId, cancellationToken);
                var returnNumberDisplay = _mapper.Map<GetSalesReturnNumberQueryResponse>(returnNumber.Data);
                return Result<GetSalesReturnNumberQueryResponse>.Success(returnNumberDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while Getting Sales return number by company{request.companyId}");
            }

        }
    }
}
