using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.DayBook.FilterSalesReturnDayBook
{
    public  class FilterSalesReturnDayBookQueryHandler : IRequestHandler<FilterSalesReturnDayBookQuery, Result<PagedResult<FilterSalesReturnDayBookQueryResponse>>>
    {
        private readonly ISalesReportService _salesReportService;
        private readonly IMapper _mapper;

        public FilterSalesReturnDayBookQueryHandler(ISalesReportService salesReportService,IMapper mapper)
        {
            _salesReportService = salesReportService;
            _mapper = mapper;

        }

        public async Task<Result<PagedResult<FilterSalesReturnDayBookQueryResponse>>> Handle(FilterSalesReturnDayBookQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterSalesReturnDayBook = await _salesReportService.GetFilterSalesReturnDayBook(request.PaginationRequest, request.FilterSalesReturnDayBookDto);

                if (!filterSalesReturnDayBook.IsSuccess || filterSalesReturnDayBook.Data == null)
                {
                    return Result<PagedResult<FilterSalesReturnDayBookQueryResponse>>.Failure(filterSalesReturnDayBook.Message);
                }

                var filterSalesReturnDayBookResult = _mapper.Map<PagedResult<FilterSalesReturnDayBookQueryResponse>>(filterSalesReturnDayBook.Data);

                return Result<PagedResult<FilterSalesReturnDayBookQueryResponse>>.Success(filterSalesReturnDayBookResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterSalesReturnDayBookQueryResponse>>.Failure(
                    $"An error occurred while fetching sales Return Day Book  by date: {ex.Message}");
            }
        }
    }
}
