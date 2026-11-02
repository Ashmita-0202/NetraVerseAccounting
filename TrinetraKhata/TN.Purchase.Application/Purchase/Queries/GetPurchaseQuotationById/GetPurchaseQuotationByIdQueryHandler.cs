using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Purchase.Application.Purchase.Queries.PurchaseDetailsById;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Queries.GetPurchaseQuotationById
{
    public  class GetPurchaseQuotationByIdQueryHandler: IRequestHandler<GetPurchaseQuotationByIdQuery, Result<GetPurchaseQuotationByIdQueryResponse>>
    {
        private readonly IPurchaseDetailsServices _services;
        private readonly IMapper _mapper;

        public GetPurchaseQuotationByIdQueryHandler(IPurchaseDetailsServices services,IMapper mapper)
        {
            _services= services;
            _mapper = mapper;
        }

        public async Task<Result<GetPurchaseQuotationByIdQueryResponse>> Handle(GetPurchaseQuotationByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var purchaseQuotation = await _services.GetPurchaseQuotationById(request.id);

                return Result<GetPurchaseQuotationByIdQueryResponse>.Success(purchaseQuotation.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Purchase Quotation by Id", ex);

            }
        }
    }
}
