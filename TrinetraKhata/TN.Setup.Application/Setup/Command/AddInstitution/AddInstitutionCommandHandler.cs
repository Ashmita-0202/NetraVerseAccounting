using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.AddMenu;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AddInstitution
{
    public class AddInstitutionCommandHandler : IRequestHandler<AddInstitutionCommand, Result<AddInstitutionResponse>>
    {
        private readonly IValidator<AddInstitutionCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IInstitutionServices _institutionServices;

        public AddInstitutionCommandHandler(IValidator<AddInstitutionCommand> validator,IMapper mapper,IInstitutionServices institutionServices)
        {
            _validator=validator;
            _mapper=mapper;
            _institutionServices=institutionServices;
        }
        public async Task<Result<AddInstitutionResponse>> Handle(AddInstitutionCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddInstitutionResponse>.Failure(errors);
                }

                var addMenu = await _institutionServices.Add(request);

                if (addMenu.Errors.Any())
                {
                    var errors = string.Join(", ", addMenu.Errors);
                    return Result<AddInstitutionResponse>.Failure(errors);
                }

                if (addMenu is null || !addMenu.IsSuccess)
                {
                    return Result<AddInstitutionResponse>.Failure("Add modules failed");
                }

                var institutionDisplays = _mapper.Map<AddInstitutionResponse>(request);
                return Result<AddInstitutionResponse>.Success(institutionDisplays);


            }
            catch (Exception ex)
            {
              throw new Exception("An error occur while adding institution",ex);
            
            }
        }
    }
}
