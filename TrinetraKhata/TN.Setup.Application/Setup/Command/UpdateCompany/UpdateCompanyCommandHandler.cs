using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.UpdateOrganization;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateCompany
{
    public class UpdateCompanyCommandHandler:IRequestHandler<UpdateCompanyCommand,Result<UpdateCompanyResponse>>
    {
        private readonly IValidator<UpdateCompanyCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ICompanyServices _companyServices;

        public UpdateCompanyCommandHandler(IValidator<UpdateCompanyCommand> validator,IMapper mapper,ICompanyServices companyServices)
        { 
        _validator=validator;
            _mapper=mapper; 
            _companyServices=companyServices;

        }

        public async Task<Result<UpdateCompanyResponse>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateCompanyResponse>.Failure(errors);

                }

                var updateCompany = await _companyServices.Update(request.id, request);

                if (updateCompany.Errors.Any())
                {
                    var errors = string.Join(", ", updateCompany.Errors);
                    return Result<UpdateCompanyResponse>.Failure(errors);
                }

                if (updateCompany is null || !updateCompany.IsSuccess)
                {
                    return Result<UpdateCompanyResponse>.Failure("Updates modules failed");
                }

                var companyDisplay = _mapper.Map<UpdateCompanyResponse>(updateCompany.Data);
                return Result<UpdateCompanyResponse>.Success(companyDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating Company", ex);

            }
        }
    }
}
