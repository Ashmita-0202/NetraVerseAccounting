using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.ServiceInterface;
using TN.Sales.Application.Sales.Queries.FilterSalesQuotationByDate;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Purchase.Application.Purchase.Queries.FilterPurchaseQuotationByDate
{
    public class FilterPurchaseQuotationQueryHandler : IRequestHandler<FilterPurchaseQuotationQuery, Result<PagedResult<FilterPurchaseQuotationQueryResponse>>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;
        private readonly IMapper _mapper;

        public FilterPurchaseQuotationQueryHandler(IPurchaseDetailsServices purchaseDetailsServices, IMapper mapper)
        {
            _purchaseDetailsServices = purchaseDetailsServices;
            _mapper = mapper;
            
        }
        public async Task<Result<PagedResult<FilterPurchaseQuotationQueryResponse>>> Handle(FilterPurchaseQuotationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterPurchaseQuotation = await _purchaseDetailsServices.GetPurchaseQuotationFilter(request.PaginationRequest, request.FilterPurchaseDetailsDTOs);

                if (!filterPurchaseQuotation.IsSuccess || filterPurchaseQuotation.Data == null)
                {
                    return Result<PagedResult<FilterPurchaseQuotationQueryResponse>>.Failure(filterPurchaseQuotation.Message);
                }

                var filterPurchaseResult = _mapper.Map<PagedResult<FilterPurchaseQuotationQueryResponse>>(filterPurchaseQuotation.Data);

                return Result<PagedResult<FilterPurchaseQuotationQueryResponse>>.Success(filterPurchaseResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterPurchaseQuotationQueryResponse>>.Failure(
                    $"An error occurred while fetching sales details  by date: {ex.Message}");
            }
        }
    }
}
