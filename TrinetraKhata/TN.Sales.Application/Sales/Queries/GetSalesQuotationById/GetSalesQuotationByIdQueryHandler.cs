using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Sales.Application.Sales.Queries.GetSalesQuotationById
{
    public  class GetSalesQuotationByIdQueryHandler:IRequestHandler<GetSalesQuotationByIdQuery,Result<GetSalesQuotationByIdQueryResponse>>
    {
        private readonly ISalesDetailsServices _services;
        private readonly IMapper _mapper;

        public GetSalesQuotationByIdQueryHandler(ISalesDetailsServices services,IMapper mapper)
        {
            _services=services;
            _mapper= mapper;
        }

        public async Task<Result<GetSalesQuotationByIdQueryResponse>> Handle(GetSalesQuotationByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var salesQuotation = await _services.GetSalesQuotationById(request.id);

                return Result<GetSalesQuotationByIdQueryResponse>.Success(salesQuotation.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching sales Quotation by Id", ex);

            }
        }
    }
}
