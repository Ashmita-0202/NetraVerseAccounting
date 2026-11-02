using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;


namespace TN.Transactions.Application.Transactions.Queries.GetPaymentById
{
    public class GetPaymentByIdQueryHandler:IRequestHandler<GetPaymentByIdQuery,Result<GetPaymentByIdQueryResponse>>
    {
        private readonly IPaymentsServices _paymentsServices;
        private readonly IMapper _mapper;

        public GetPaymentByIdQueryHandler(IPaymentsServices paymentsServices,IMapper mapper)
        {
            _paymentsServices=paymentsServices;
            _mapper=mapper;
        
        }

        public async Task<Result<GetPaymentByIdQueryResponse>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var payment = await _paymentsServices.GetPaymentById(request.id);

                return Result<GetPaymentByIdQueryResponse>.Success(payment.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching payment by Id", ex);

            }
        }
    }
}
