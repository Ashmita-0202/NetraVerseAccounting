using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;


namespace TN.Sales.Application.SalesReturn.Queries.GetSalesReturnDetailsById
{
    public  class GetSalesReturnDetailsByIdQueryHandler:IRequestHandler<GetSalesReturnDetailsByIdQuery,Result<GetSalesReturnDetailsByIdQueryResponse>>
    {
        private readonly ISalesReturnServices _salesReturnServices;
        private readonly IMapper _mapper;

        public GetSalesReturnDetailsByIdQueryHandler(ISalesReturnServices salesReturnServices,IMapper mapper)
        {
            _salesReturnServices=salesReturnServices;
            _mapper=mapper;
        }

        public  async Task<Result<GetSalesReturnDetailsByIdQueryResponse>> Handle(GetSalesReturnDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var salesReturnDetailsById = await _salesReturnServices.GetSalesReturnDetailsById(request.id);
                return Result<GetSalesReturnDetailsByIdQueryResponse>.Success(salesReturnDetailsById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching salesReturn Details by using id", ex);
            }
        }
    }
}
