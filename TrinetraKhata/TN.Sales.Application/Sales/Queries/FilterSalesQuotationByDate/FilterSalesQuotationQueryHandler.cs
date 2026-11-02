using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.Sales.Queries.FilterSalesDetailsByDate;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Sales.Application.Sales.Queries.FilterSalesQuotationByDate
{
    public class FilterSalesQuotationQueryHandler : IRequestHandler<FilterSalesQuotationQuery, Result<PagedResult<FilterSalesQuotationQueryResponse>>>
    {
        private readonly ISalesDetailsServices _salesDetailsServices;
        private readonly IMapper _mapper;


        public FilterSalesQuotationQueryHandler(ISalesDetailsServices salesDetailsServices, IMapper mapper)
        {
            _salesDetailsServices = salesDetailsServices;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<FilterSalesQuotationQueryResponse>>> Handle(FilterSalesQuotationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterSalesQuotation = await _salesDetailsServices.GetFilterSalesQuotation(request.PaginationRequest, request.FilterSalesDetailsDTOs);

                if (!filterSalesQuotation.IsSuccess || filterSalesQuotation.Data == null)
                {
                    return Result<PagedResult<FilterSalesQuotationQueryResponse>>.Failure(filterSalesQuotation.Message);
                }

                var filterSalesResult = _mapper.Map<PagedResult<FilterSalesQuotationQueryResponse>>(filterSalesQuotation.Data);

                return Result<PagedResult<FilterSalesQuotationQueryResponse>>.Success(filterSalesResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterSalesQuotationQueryResponse>>.Failure(
                    $"An error occurred while fetching sales details  by date: {ex.Message}");
            }
        }
    }
}
