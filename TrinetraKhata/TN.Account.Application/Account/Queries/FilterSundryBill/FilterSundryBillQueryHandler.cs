using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.FilterSundryBill
{
    public  class FilterSundryBillQueryHandler:IRequestHandler<FilterSundryBillQuery,Result<PagedResult<FilterSundryBillQueryResponse>>>
    {
        private readonly IBillSundryServices _billSundryServices;
        private readonly IMapper _mapper;

        public FilterSundryBillQueryHandler(IBillSundryServices billSundryServices,IMapper mapper)
        {
            _billSundryServices= billSundryServices;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<FilterSundryBillQueryResponse>>> Handle(FilterSundryBillQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _billSundryServices.FilterBillSundry(request.PaginationRequest, request.FilterSundryBillDto);

                var sundryBillResult = _mapper.Map<PagedResult<FilterSundryBillQueryResponse>>(result.Data);

                return Result<PagedResult<FilterSundryBillQueryResponse>>.Success(sundryBillResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterSundryBillQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
