using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.GetAllStockTransferDetails
{
    public  class GetAllStockTransferDetailsQueryHandler:IRequestHandler<GetAllStockTransferDetailsQuery,Result<PagedResult<GetAllStockTransferDetailsQueryResponse>>>
    {
        private readonly IStockTransferDetailsServices _services;
        private readonly IMapper _mapper;

        public GetAllStockTransferDetailsQueryHandler(IStockTransferDetailsServices services,IMapper mapper)
        {
            _services=services;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetAllStockTransferDetailsQueryResponse>>> Handle(GetAllStockTransferDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allStockTransfer = await _services.GetAllStockTransferDetail(request.PaginationRequest, cancellationToken);
                var allStockTransferDisplay = _mapper.Map<PagedResult<GetAllStockTransferDetailsQueryResponse>>(allStockTransfer.Data);
                return Result<PagedResult<GetAllStockTransferDetailsQueryResponse>>.Success(allStockTransferDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all stock Transfer ", ex);
            }
        }
    }
}
