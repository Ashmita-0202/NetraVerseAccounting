using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Queries.UnitsById
{
   public sealed class GetUnitsByIdQueryHandler:IRequestHandler<GetUnitsByIdQuery,Result<GetUnitsByIdQueryResponse>>
    {
        private readonly IUnitsServices _unitsServices;
        private readonly IMapper _mapper;

        public GetUnitsByIdQueryHandler(IUnitsServices unitsServices,IMapper mapper) 
        {
            _unitsServices=unitsServices;
            _mapper=mapper;
        }

        public async Task<Result<GetUnitsByIdQueryResponse>> Handle(GetUnitsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var unitsById = await _unitsServices.GetUnitsById(request.id);
                return Result<GetUnitsByIdQueryResponse>.Success(unitsById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Units by using id", ex);
            }
        }
    }
}
