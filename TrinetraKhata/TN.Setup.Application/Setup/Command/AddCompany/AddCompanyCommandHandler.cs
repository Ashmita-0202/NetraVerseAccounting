using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.AddInstitution;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AddCompany
{
    public class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand, Result<AddCompanyResponse>>
    {
        private readonly IValidator<AddCompanyCommand> _validator;
        private readonly ICompanyServices _companyServices;
        private readonly IMapper _mapper;

        public AddCompanyCommandHandler(IValidator<AddCompanyCommand> validator, IMapper mapper,ICompanyServices companyServices)
        {
            _validator=validator;
            _companyServices=companyServices;
            _mapper=mapper;
        }

        public async Task<Result<AddCompanyResponse>> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddCompanyResponse>.Failure(errors);
                }

                var addMenu = await _companyServices.Add(request);

                if (addMenu.Errors.Any())
                {
                    var errors = string.Join(", ", addMenu.Errors);
                    return Result<AddCompanyResponse>.Failure(errors);
                }

                if (addMenu is null || !addMenu.IsSuccess)
                {
                    return Result<AddCompanyResponse>.Failure("Add company failed");
                }

                var companyDisplays = _mapper.Map<AddCompanyResponse>(request);
                return Result<AddCompanyResponse>.Success(companyDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occur while adding Company", ex);

            }
        }
    }
}
