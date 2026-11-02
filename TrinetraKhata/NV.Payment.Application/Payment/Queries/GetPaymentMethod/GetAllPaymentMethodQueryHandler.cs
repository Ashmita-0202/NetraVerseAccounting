using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NV.Payment.Application.Payment.Queries.GetPayment;
using NV.Payment.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace NV.Payment.Application.Payment.Queries.GetPaymentMethod
{
    public class GetAllPaymentMethodQueryHandler:IRequestHandler<GetAllPaymentMethodQuery,Result<PagedResult<GetAllPaymentMethodQueryResponse>>>
    {
        private readonly IPaymentMethodService _service;
        private readonly IMapper _mapper;

        public GetAllPaymentMethodQueryHandler(IPaymentMethodService service,IMapper mapper) 
        {
            _service=service;
            _mapper=mapper;
        
        }

        public async Task<Result<PagedResult<GetAllPaymentMethodQueryResponse>>> Handle(GetAllPaymentMethodQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allPaymentMethod = await _service.GetAllPaymentMethod(request.PaginationRequest, cancellationToken);
                var allPaymentMethodDisplay = _mapper.Map<PagedResult<GetAllPaymentMethodQueryResponse>>(allPaymentMethod.Data);
                return Result<PagedResult<GetAllPaymentMethodQueryResponse>>.Success(allPaymentMethodDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all PaymentMethod ", ex);
            }
        }
    }
}
