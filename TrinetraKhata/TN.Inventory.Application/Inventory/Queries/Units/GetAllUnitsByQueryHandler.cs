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

namespace TN.Inventory.Application.Inventory.Queries.Units
{
  public sealed class  GetAllUnitsByQueryHandler:IRequestHandler<GetAllUnitsByQuery,Result<PagedResult<GetAllUnitsByQueryResponse>>>
    {
        private IUnitsServices _unitsServices;
        private readonly IMapper _mapper;

        public GetAllUnitsByQueryHandler(IUnitsServices unitsServices,IMapper mapper)
        {
            _unitsServices=unitsServices;
            _mapper=mapper;
        }

        public async  Task<Result<PagedResult<GetAllUnitsByQueryResponse>>> Handle(GetAllUnitsByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allUnits = await _unitsServices.GetAllUnits(request.PaginationRequest, cancellationToken);
                var allUnitsDisplay = _mapper.Map<PagedResult<GetAllUnitsByQueryResponse>>(allUnits.Data);
                return Result<PagedResult<GetAllUnitsByQueryResponse>>.Success(allUnitsDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all customer", ex);
            }
        }
    }
}
