using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Shared.Application.Shared.Queries.GetAllCurrentFiscalYear
{
    public  class GetAllFiscalYearQueryHandler: IRequestHandler<GetAllFiscalYearQuery, Result<PagedResult<GetAllFiscalYearQueryResponse>>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public GetAllFiscalYearQueryHandler(ISettingServices settingServices,IMapper mapper)
        {
            _settingServices = settingServices;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetAllFiscalYearQueryResponse>>> Handle(GetAllFiscalYearQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allFiscalYear = await _settingServices.GetAllFiscalYear(request.PaginationRequest, cancellationToken);
                var allFiscalYearDisplay = _mapper.Map<PagedResult<GetAllFiscalYearQueryResponse>>(allFiscalYear.Data);
                return Result<PagedResult<GetAllFiscalYearQueryResponse>>.Success(allFiscalYearDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all fiscal year", ex);
            }
        }
    }
}
