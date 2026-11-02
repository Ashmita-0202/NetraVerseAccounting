using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Queries.GetStockTransferDetailsById
{
    public  class GetStockTransferDetailsByIdQueryHandler:IRequestHandler<GetStockTransferDetailsByIdQuery,Result<GetStockTransferDetailsByIdQueryResponse>>
    {
        private readonly IStockTransferDetailsServices _services;
        private readonly IMapper _mapper;

        public GetStockTransferDetailsByIdQueryHandler(IStockTransferDetailsServices services,IMapper mapper)
        {
            _services=services;
            _mapper=mapper;
        }

        public async Task<Result<GetStockTransferDetailsByIdQueryResponse>> Handle(GetStockTransferDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var stockTransferById = await _services.GetStockTransferDetailsById(request.id);
                return Result<GetStockTransferDetailsByIdQueryResponse>.Success(stockTransferById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching stock transferDetails by using id", ex);
            }
        }
    }
}
