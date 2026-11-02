using AutoMapper;
using MediatR;
using TN.Inventory.Application.Inventory.Queries.Units;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.StockExpiryNotification
{
    public class StockExpiryNotificationQueryHandler : IRequestHandler<StockExpiryNotificationQuery, Result<PagedResult<StockExpiryNotificationResponse>>>
    {
        private readonly IItemsServices _itemsServices;
        private readonly IMapper _mapper;

        public StockExpiryNotificationQueryHandler(IItemsServices itemsServices, IMapper mapper)
        {
            _mapper = mapper;
            _itemsServices = itemsServices;

        }

        public async Task<Result<PagedResult<StockExpiryNotificationResponse>>> Handle(StockExpiryNotificationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allUnits = await _itemsServices.GetStockExpiryNotification(request.paginationRequest, cancellationToken);
                var allUnitsDisplay = _mapper.Map<PagedResult<StockExpiryNotificationResponse>>(allUnits.Data);
                return Result<PagedResult<StockExpiryNotificationResponse>>.Success(allUnitsDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all customer", ex);
            }
        }
    }
}
