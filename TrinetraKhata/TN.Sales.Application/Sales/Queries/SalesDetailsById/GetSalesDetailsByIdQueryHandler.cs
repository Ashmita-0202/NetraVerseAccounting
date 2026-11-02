using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Sales.Application.Sales.Queries.SalesDetailsById
{
 public class GetSalesDetailsByIdQueryHandler:IRequestHandler<GetSalesDetailsByIdQuery, Result<GetSalesDetailsByIdQueryResponse>>
    {
        private readonly ISalesDetailsServices _salesDetailsServices;
        private readonly IMapper _mapper;

        public GetSalesDetailsByIdQueryHandler(ISalesDetailsServices salesDetailsServices,IMapper mapper)
        {
            _salesDetailsServices=salesDetailsServices;
            _mapper=mapper;
        }

        public async Task<Result<GetSalesDetailsByIdQueryResponse>> Handle(GetSalesDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var salesDetails = await _salesDetailsServices.GetSalesDetailsById(request.id);

                return Result<GetSalesDetailsByIdQueryResponse>.Success(salesDetails.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching sales Details by Id", ex);

            }
        }
    }
}
