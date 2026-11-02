using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Transactions.Application.ServiceInterface;
using TN.Transactions.Application.Transactions.Queries.FilterReceiptByDate;

namespace TN.Transactions.Application.Transactions.Queries.FilterPaymentByDate
{
    public class GetFilterPaymentQueryHandler : IRequestHandler<GetFilterPaymentQuery, Result<PagedResult<GetFilterPaymentQueryResponse>>>
    {
        private readonly IPaymentsServices _paymentsServices;
        private readonly IMapper _mapper;

        public GetFilterPaymentQueryHandler(IPaymentsServices paymentsServices,IMapper mapper)
        {
            _paymentsServices = paymentsServices;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetFilterPaymentQueryResponse>>> Handle(GetFilterPaymentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterPayment = await _paymentsServices.GetPaymentFilter(request.PaginationRequest, request.FilterPaymentDto);

                if (!filterPayment.IsSuccess || filterPayment.Data == null)
                {
                    return Result<PagedResult<GetFilterPaymentQueryResponse>>.Failure(filterPayment.Message);
                }

                var filterPaymentResult = _mapper.Map<PagedResult<GetFilterPaymentQueryResponse>>(filterPayment.Data);

                return Result<PagedResult<GetFilterPaymentQueryResponse>>.Success(filterPaymentResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetFilterPaymentQueryResponse>>.Failure(
                    $"An error occurred while fetching payment  by date: {ex.Message}");
            }
        }
    }
}
