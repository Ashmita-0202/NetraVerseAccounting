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

namespace TN.Shared.Application.Shared.Queries.GetSelectedFiscalYear
{
    public class GetSelectedFiscalYearQueryHandler : IRequestHandler<GetSelectedFiscalYearQuery, Result<List<GetSelectedFiscalYearQueryResponse>>>
    {
        private readonly IFiscalYearService _fiscalYearServices;
        private readonly IMapper _mapper;

        public GetSelectedFiscalYearQueryHandler(IMapper mapper, IFiscalYearService fiscalYearService)
        {
            _mapper = mapper;
            _fiscalYearServices = fiscalYearService;
            
        }
        public async Task<Result<List<GetSelectedFiscalYearQueryResponse>>> Handle(GetSelectedFiscalYearQuery request, CancellationToken cancellationToken)
        {
                var selectedFiscalYear = await _fiscalYearServices.GetSelectedFiscalYear(request.PaginationRequest, cancellationToken);

                if (!selectedFiscalYear.IsSuccess)
                {
                    return Result<List<GetSelectedFiscalYearQueryResponse>>.Failure(selectedFiscalYear.Errors.ToArray());
                }
                var selectedFiscalYearDisplay = _mapper.Map<List<GetSelectedFiscalYearQueryResponse>>(selectedFiscalYear.Data);
                return Result<List<GetSelectedFiscalYearQueryResponse>>.Success(selectedFiscalYearDisplay);
        }
    }
}
