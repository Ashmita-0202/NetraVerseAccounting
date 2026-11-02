using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Sales.Application.Sales.Queries.SalesDetailsById;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Sales.Application.Sales.Queries.GetSalesDetailsByRefNo
{
    public class GetSalesDetailsQueryHandler:IRequestHandler<GetSalesDetailsQuery,Result<GetSalesDetailsQueryResponse>>
    {
        private readonly ISalesDetailsServices _services;
        private readonly IMapper _mapper;

        public GetSalesDetailsQueryHandler(ISalesDetailsServices services,IMapper mapper) 
        {
            _services = services;
            _mapper = mapper;
        
        }

        public async Task<Result<GetSalesDetailsQueryResponse>> Handle(GetSalesDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var salesDetails = await _services.GetSalesDetailsByRefNo(request.referenceNumber);

                return Result<GetSalesDetailsQueryResponse>.Success(salesDetails.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching sales Details by reference number", ex);

            }
        }
    }
}
