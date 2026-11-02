using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.DeleteCompany
{
    public class DeleteCompanyCommandHandler:IRequestHandler<DeleteCompanyCommand,Result<bool>>
    {
        private readonly ICompanyServices _companyServices;
        private readonly IMapper _mapper;

        public DeleteCompanyCommandHandler(ICompanyServices companyServices, IMapper mapper)
        {

            _companyServices=companyServices;
            _mapper=mapper;
        }

        public async Task<Result<bool>> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteCompany = await _companyServices.Delete(request.id, cancellationToken);
                if (deleteCompany is null)
                {
                    return Result<bool>.Failure("NotFound", "Organization not Found");
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
