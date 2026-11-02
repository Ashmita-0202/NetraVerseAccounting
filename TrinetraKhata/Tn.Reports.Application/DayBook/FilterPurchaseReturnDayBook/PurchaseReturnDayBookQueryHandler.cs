using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Reports.Application.DayBook.FilterPurchaseDayBook;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.DayBook.FilterPurchaseReturnDayBook
{
    public  class PurchaseReturnDayBookQueryHandler:IRequestHandler<PurchaseReturnDayBookQuery,Result<PagedResult<PurchaseReturnDayBookQueryResponse>>>
    {
        private readonly IPurchaseReportService _purchaseReportService;
        private readonly IMapper _mapper;

        public PurchaseReturnDayBookQueryHandler(IPurchaseReportService purchaseReportService,IMapper mapper)
        {
            _purchaseReportService = purchaseReportService;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<PurchaseReturnDayBookQueryResponse>>> Handle(PurchaseReturnDayBookQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var filterPurchaseReturnDayBook = await _purchaseReportService.GetPurchaseReturnDayBook(request.PaginationRequest, request.PurchaseReturnDayBookDto);

                if (!filterPurchaseReturnDayBook.IsSuccess || filterPurchaseReturnDayBook.Data == null)
                {
                    return Result<PagedResult<PurchaseReturnDayBookQueryResponse>>.Failure(filterPurchaseReturnDayBook.Message);
                }

                var purchaseReturnDayBookResult = _mapper.Map<PagedResult<PurchaseReturnDayBookQueryResponse>>(filterPurchaseReturnDayBook.Data);

                return Result<PagedResult<PurchaseReturnDayBookQueryResponse>>.Success(purchaseReturnDayBookResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<PurchaseReturnDayBookQueryResponse>>.Failure(
                    $"An error occurred while fetching Purchase Return Day Book  by date: {ex.Message}");
            }
        }
    }
}
