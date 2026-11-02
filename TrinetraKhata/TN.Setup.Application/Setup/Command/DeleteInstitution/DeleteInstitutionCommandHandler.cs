using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.DeleteInstitution
{
   public class DeleteInstitutionCommandHandler:IRequestHandler<DeleteInstitutionCommand, Result<bool>>
    {
        private readonly IInstitutionServices _institutionServices;
        private readonly IMapper _mapper;

        public DeleteInstitutionCommandHandler(IInstitutionServices institutionServices,IMapper mapper) 
        { 
            _institutionServices=institutionServices;
            _mapper=mapper;
        
        }

        public async Task<Result<bool>> Handle(DeleteInstitutionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteInstitution = await _institutionServices.Delete(request.Id, cancellationToken);
                if (deleteInstitution is null)
                {
                    return Result<bool>.Failure("NotFound", "Institution not Found");
                }
                return Result<bool>.Success(true);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.Id}", ex);
            }
        }
    }
}
