using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NV.Payment.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace NV.Payment.Application.Payment.Queries.FilterPaymentMethod
{
    public  class GetFilterPaymentMethodQueryHandler:IRequestHandler<GetFilterPaymentMethodQuery,Result<PagedResult<GetFilterPaymentMethodResponse>>>
    {
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IMapper _mapper;

        public GetFilterPaymentMethodQueryHandler(IPaymentMethodService paymentMethodService,IMapper mapper)
        {
            _paymentMethodService = paymentMethodService;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetFilterPaymentMethodResponse>>> Handle(GetFilterPaymentMethodQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _paymentMethodService.GetPaymentMethodFilter(request.PaginationRequest, request.FilterPaymentMethodDto);

                var filterPaymentMethod = _mapper.Map<PagedResult<GetFilterPaymentMethodResponse>>(result.Data);

                return Result<PagedResult<GetFilterPaymentMethodResponse>>.Success(filterPaymentMethod);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetFilterPaymentMethodResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
