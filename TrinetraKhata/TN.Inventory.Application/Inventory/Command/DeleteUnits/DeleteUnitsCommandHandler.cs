using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.DeleteUnits
{
 public class DeleteUnitsCommandHandler:IRequestHandler<DeleteUnitsCommand,Result<bool>>
    {
        private readonly IUnitsServices _unitsServices;
        private readonly IMapper _mapper;

        public DeleteUnitsCommandHandler(IUnitsServices unitsServices,IMapper mapper) 
        {
            _unitsServices=unitsServices;
            _mapper=mapper;
        }

        public async Task<Result<bool>> Handle(DeleteUnitsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteUnits = await _unitsServices.Delete(request.id, cancellationToken);
                if (deleteUnits is null)
                {
                    return Result<bool>.Failure("NotFound", "units not Found");
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
