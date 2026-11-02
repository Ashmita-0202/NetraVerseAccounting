using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Reports.Application.DayBook.FilterSalesDayBook;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.DayBook.CashBook
{
    public class CashDayBookQueryHandler :IRequestHandler<CashDayBookQuery,Result<PagedResult<CashDayBookQueryResponse>>>
    {
        private readonly IPurchaseReportService _purchaseReportService;
        private readonly IMapper _mapper;

        public CashDayBookQueryHandler(IPurchaseReportService purchaseReportService,IMapper mapper)
        {
            _purchaseReportService = purchaseReportService;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<CashDayBookQueryResponse>>> Handle(CashDayBookQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dayBook = await _purchaseReportService.GetCashDayBook(request.PaginationRequest, request.CashDayBookDto);

                if (!dayBook.IsSuccess || dayBook.Data == null)
                {
                    return Result<PagedResult<CashDayBookQueryResponse>>.Failure(dayBook.Message);
                }

                var dayBookResult = _mapper.Map<PagedResult<CashDayBookQueryResponse>>(dayBook.Data);

                return Result<PagedResult<CashDayBookQueryResponse>>.Success(dayBookResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<CashDayBookQueryResponse>>.Failure(
                    $"An error occurred while fetching cash Day Book  by date: {ex.Message}");
            }
        }
    }
}
