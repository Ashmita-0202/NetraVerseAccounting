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

namespace TN.Inventory.Application.Inventory.Queries.ItemGroup
{
   public sealed class GetAllItemGroupByQueryHandler:IRequestHandler<GetAllItemGroupByQuery, Result<PagedResult<GetAllItemGroupByQueryResponse>>>
    {
        private readonly IItemGroupServices _itemGroupServices;
        private readonly IMapper _mapper;

        public GetAllItemGroupByQueryHandler(IItemGroupServices itemGroupServices, IMapper mapper)
        { 
            _itemGroupServices=itemGroupServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetAllItemGroupByQueryResponse>>> Handle(GetAllItemGroupByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allItemGroup = await _itemGroupServices.GetAllItemGroup(request.PaginationRequest, cancellationToken);
                var allItemGroupDisplay = _mapper.Map<PagedResult<GetAllItemGroupByQueryResponse>>(allItemGroup.Data);
                return Result<PagedResult<GetAllItemGroupByQueryResponse>>.Success(allItemGroupDisplay);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all customer", ex);
            }
        }
    }
}
