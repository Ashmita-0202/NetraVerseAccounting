using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.DeleteConversionFactor
{
    public class DeleteConversionFactorCommandHandler:IRequestHandler<DeleteConversionFactorCommand, Result<bool>>
    {
        private readonly IConversionFactorServices _conversionFactorServices;
        private readonly IMapper _mapper;

        public DeleteConversionFactorCommandHandler(IConversionFactorServices conversionFactorServices, IMapper mapper)
        {
            _conversionFactorServices=conversionFactorServices;
            _mapper=mapper;
        
        }

        public async Task<Result<bool>> Handle(DeleteConversionFactorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteConversionFactor = await _conversionFactorServices.Delete(request.id, cancellationToken);
                if (deleteConversionFactor is null)
                {
                    return Result<bool>.Failure("NotFound", "conversion Factor are not Found");
                }
                return Result<bool>.Success(true);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.id}", ex);
            }
        }
    }
}
