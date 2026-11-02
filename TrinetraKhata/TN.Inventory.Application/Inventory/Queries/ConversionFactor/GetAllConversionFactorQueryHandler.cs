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

namespace TN.Inventory.Application.Inventory.Queries.ConversionFactor
{
   public class  GetAllConversionFactorQueryHandler:IRequestHandler<GetAllConversionFactorQuery, Result<PagedResult<GetAllConversionFactorQueryResponse>>>
    {
        private readonly IConversionFactorServices _conversionFactorServices;
        private readonly IMapper _mapper;

        public GetAllConversionFactorQueryHandler(IConversionFactorServices conversionFactorServices, IMapper mapper)
        {
            _conversionFactorServices=conversionFactorServices;
            _mapper=mapper;
        
        }

        public async Task<Result<PagedResult<GetAllConversionFactorQueryResponse>>> Handle(GetAllConversionFactorQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allConversionFactor = await _conversionFactorServices.GetAllConversionFactor(request.PaginationRequest, cancellationToken);
                var allConversionDisplay = _mapper.Map<PagedResult<GetAllConversionFactorQueryResponse>>(allConversionFactor.Data);
                return Result<PagedResult<GetAllConversionFactorQueryResponse>>.Success(allConversionDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all conversion factor", ex);
            }
        }
    }
}
