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

namespace TN.Reports.Application.DayBook.FilterSalesDayBook
{
    public class FilterSalesDayBookQueryHandler : IRequestHandler<FilterSalesDayBookQuery, Result<PagedResult<FilterSalesDayBookQueryResponse>>>
    {
        private readonly ISalesReportService _salesReportService;
        private readonly IMapper _mapper;

        public FilterSalesDayBookQueryHandler(ISalesReportService salesReportService, IMapper mapper)
        {
            _salesReportService = salesReportService;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<FilterSalesDayBookQueryResponse>>> Handle(FilterSalesDayBookQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var filterSalesDayBook = await _salesReportService.GetFilterSalesDayBook(request.PaginationRequest, request.FilterSalesDayBookDto);

                if (!filterSalesDayBook.IsSuccess || filterSalesDayBook.Data == null)
                {
                    return Result<PagedResult<FilterSalesDayBookQueryResponse>>.Failure(filterSalesDayBook.Message);
                }

                var filterSalesDayBookResult = _mapper.Map<PagedResult<FilterSalesDayBookQueryResponse>>(filterSalesDayBook.Data);

                return Result<PagedResult<FilterSalesDayBookQueryResponse>>.Success(filterSalesDayBookResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterSalesDayBookQueryResponse>>.Failure(
                    $"An error occurred while fetching sales Day Book  by date: {ex.Message}");
            }
        }
    }
}
