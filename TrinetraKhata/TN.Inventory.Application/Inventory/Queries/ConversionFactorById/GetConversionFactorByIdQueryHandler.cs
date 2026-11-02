using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.Inventory.Queries.UnitsById;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Queries.ConversionFactorById
{
   public sealed class GetConversionFactorByIdQueryHandler:IRequestHandler<GetConversionFactorByIdQuery,Result<GetConversionFactorByIdResponse>>
    {
        private readonly IConversionFactorServices _conversionFactorServices;
        private readonly IMapper _mapper;

        public GetConversionFactorByIdQueryHandler(IConversionFactorServices conversionFactorServices,IMapper mapper)
        { 
            _conversionFactorServices=conversionFactorServices;
            _mapper=mapper;
        
        }

        public async Task<Result<GetConversionFactorByIdResponse>> Handle(GetConversionFactorByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var conversionFactorById = await _conversionFactorServices.GetConversionFactorById(request.id);
                return Result<GetConversionFactorByIdResponse>.Success(conversionFactorById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching conversion factor by using id", ex);
            }
        }
    }
}
