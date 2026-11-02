using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Queries.GetAllCurrentFiscalYear;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Shared.Application.Shared.Queries.FiscalYearStartDate
{
    public class FiscalYearStartDateQueryHandler : IRequestHandler<FiscalYearStartDateQuery, Result<FiscalYearStartDateResponse>>
    {
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IMapper _mapper;

        public FiscalYearStartDateQueryHandler(IFiscalYearService fiscalYearService, IMapper mapper)
        {
            _fiscalYearService = fiscalYearService;
            _mapper = mapper;

        }
        public async Task<Result<FiscalYearStartDateResponse>> Handle(FiscalYearStartDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var fiscalYearStartDate = await _fiscalYearService.GetFiscalYearStartDate();
                var fiscalYearStartDateDetails = _mapper.Map<FiscalYearStartDateResponse>(fiscalYearStartDate.Data);
                return Result<FiscalYearStartDateResponse>.Success(fiscalYearStartDateDetails);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all fiscal year", ex);
            }
        }
    }
}
