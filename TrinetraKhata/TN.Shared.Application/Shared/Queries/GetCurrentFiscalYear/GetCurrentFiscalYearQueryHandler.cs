using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Queries.GetCurrentFiscalYearByCompany;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetCurrentFiscalYear
{
    public  class GetCurrentFiscalYearQueryHandler:IRequestHandler<GetCurrentFiscalYearByQuery,Result<GetCurrentFiscalYearQueryResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public GetCurrentFiscalYearQueryHandler(ISettingServices settingServices, IMapper mapper)
        {
             _settingServices = settingServices;
            _mapper = mapper;
        }

        public async Task<Result<GetCurrentFiscalYearQueryResponse>> Handle(GetCurrentFiscalYearByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var fiscalYear = await _settingServices.GetCurrentFiscalYearBy(request.companyId, cancellationToken);
                var fiscalYearDisplay = _mapper.Map<GetCurrentFiscalYearQueryResponse>(fiscalYear.Data);
                return Result<GetCurrentFiscalYearQueryResponse>.Success(fiscalYearDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while Getting Current Fiscal year by company{request.companyId}");
            }
        }
    }
}
