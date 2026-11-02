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


namespace TN.Transactions.Application.Transactions.Queries.GetAllPayments
{
    public class GetAllPaymentsQueryHandler:IRequestHandler<GetAllPaymentsQuery,Result<PagedResult<GetAllPaymentsQueryResposne>>>
    {
        private readonly IPaymentsServices _paymentsServices;
        private readonly IMapper _mapper;

        public GetAllPaymentsQueryHandler(IPaymentsServices paymentsServices,IMapper mapper) 
        {
            _paymentsServices=paymentsServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetAllPaymentsQueryResposne>>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allPayment = await _paymentsServices.GetAll(request.PaginationRequest,request.ledgerId, cancellationToken);
                var allPaymentDisplay = _mapper.Map<PagedResult<GetAllPaymentsQueryResposne>>(allPayment.Data);

                return Result<PagedResult<GetAllPaymentsQueryResposne>>.Success(allPaymentDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("an error occurred while fetching Payment", ex);

            }
        }
    }
}
