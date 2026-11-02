using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.Inventory.Queries.ConversionFactorById;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Queries.ItemGroupById
{
    public sealed class GetItemGroupByIdQueryHandler:IRequestHandler<GetItemGroupByIdQuery, Result<GetItemGroupByIdQueryResponse>>
    {
        private readonly IItemGroupServices _itemGroupServices;
        private readonly IMapper _mapper;

        public GetItemGroupByIdQueryHandler(IItemGroupServices itemGroupServices, IMapper mapper)
        { 
            _itemGroupServices= itemGroupServices;
            _mapper= mapper;

        }

        public async Task<Result<GetItemGroupByIdQueryResponse>> Handle(GetItemGroupByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var itemGroupById = await _itemGroupServices.GetItemGroupById(request.id);
                return Result<GetItemGroupByIdQueryResponse>.Success(itemGroupById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching ItemGroup by using id", ex);
            }
        }
    }
}
