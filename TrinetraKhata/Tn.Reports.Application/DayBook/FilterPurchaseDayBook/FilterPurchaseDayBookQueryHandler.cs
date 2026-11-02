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

namespace TN.Reports.Application.DayBook.FilterPurchaseDayBook
{
    public class FilterPurchaseDayBookQueryHandler : IRequestHandler<FilterPurchaseDayBookQuery, Result<PagedResult<FilterPurchaseDayBookQueryResponse>>>
    {
        private readonly IPurchaseReportService _purchaseReportService;
        private readonly IMapper _mapper;

        public FilterPurchaseDayBookQueryHandler(IPurchaseReportService purchaseReportService, IMapper mapper)
        {
            _purchaseReportService = purchaseReportService;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<FilterPurchaseDayBookQueryResponse>>> Handle(FilterPurchaseDayBookQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterPurchaseDayBook = await _purchaseReportService.GetFilterPurchaseDayBook(request.PaginationRequest, request.FilterPurchaseDayBookDto);

                if (!filterPurchaseDayBook.IsSuccess || filterPurchaseDayBook.Data == null)
                {
                    return Result<PagedResult<FilterPurchaseDayBookQueryResponse>>.Failure(filterPurchaseDayBook.Message);
                }

                var filterSalesDayBookResult = _mapper.Map<PagedResult<FilterPurchaseDayBookQueryResponse>>(filterPurchaseDayBook.Data);

                return Result<PagedResult<FilterPurchaseDayBookQueryResponse>>.Success(filterSalesDayBookResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterPurchaseDayBookQueryResponse>>.Failure(
                    $"An error occurred while fetching Purchase Day Book  by date: {ex.Message}");
            }
        }
    }
}
