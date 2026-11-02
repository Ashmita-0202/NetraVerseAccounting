using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NV.Payment.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace NV.Payment.Application.Payment.Queries.GetPaymentMethodById
{
    public  class GetPaymentMethodByIdQueryHandler:IRequestHandler<GetPaymentMethodByIdQuery,Result<GetPaymentMethodByIdQueryResponse>>
    {
        private readonly IPaymentMethodService _service;
        private readonly IMapper _mapper;

        public GetPaymentMethodByIdQueryHandler(IPaymentMethodService service,IMapper mapper)
        {
            _service=service;
            _mapper=mapper;
            
        }

        public async Task<Result<GetPaymentMethodByIdQueryResponse>> Handle(GetPaymentMethodByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var paymentMethodById = await _service.GetPaymentMethodById(request.id);
                return Result<GetPaymentMethodByIdQueryResponse>.Success(paymentMethodById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Payment method by using id", ex);
            }
        }
    }
}
