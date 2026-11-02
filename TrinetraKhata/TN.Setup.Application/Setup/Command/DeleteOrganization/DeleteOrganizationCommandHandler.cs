using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.DeleteOrganization
{
  public class DeleteOrganizationCommandHandler :IRequestHandler<DeleteOrganizationCommand,Result<bool>>
    {
        private readonly IOrganizationServices _organizationServices;
        private readonly IMapper _mapper;

        public DeleteOrganizationCommandHandler(IOrganizationServices organizationServices,IMapper mapper) 
        {
            _organizationServices=organizationServices;
            _mapper=mapper;
        
        }

        public async Task<Result<bool>> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteOrganization = await _organizationServices.Delete(request.Id, cancellationToken);
                if (deleteOrganization is null)
                {
                    return Result<bool>.Failure("NotFound", "Organization not Found");
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
