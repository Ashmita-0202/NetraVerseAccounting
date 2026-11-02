using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Queries.StockCentersById
{
    public class GetStockCenterByIdQueryHandler: IRequestHandler<GetStockCenterByIdQuery, Result<GetStockQueryByIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IStockCenterService _service;

        public GetStockCenterByIdQueryHandler(IStockCenterService service,IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }

        public async Task<Result<GetStockQueryByIdResponse>> Handle(GetStockCenterByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var stockCenterById = await _service.GetStockCenterById(request.id);
                return Result<GetStockQueryByIdResponse>.Success(stockCenterById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching stockCenter by using id", ex);
            }
        }
    }
}
