using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.UpdateCompany;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateMenu
{
    public class UpdateMenuCommandHandler : IRequestHandler<UpdateMenuCommand, Result<UpdateMenuResponse>>
    {
        private readonly IValidator<UpdateMenuCommand> _validator;
        private readonly IMenuServices _menuServices;
        private readonly IMapper _mapper;

        public UpdateMenuCommandHandler(IValidator<UpdateMenuCommand> validator, IMapper mapper, IMenuServices menuServices)
        {
            _validator = validator;
            _menuServices = menuServices;
            _mapper = mapper;

        }

        public async Task<Result<UpdateMenuResponse>> Handle(UpdateMenuCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateMenuResponse>.Failure(errors);

                }

                var updateMenu = await _menuServices.Update(request.id, request);

                if (updateMenu.Errors.Any())
                {
                    var errors = string.Join(", ", updateMenu.Errors);
                    return Result<UpdateMenuResponse>.Failure(errors);
                }

                if (updateMenu is null || !updateMenu.IsSuccess)
                {
                    return Result<UpdateMenuResponse>.Failure("Updates menu failed");
                }

                var menuDisplay = _mapper.Map<UpdateMenuResponse>(updateMenu.Data);
                return Result<UpdateMenuResponse>.Success(menuDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating Menu", ex);

            }
        }
    }
}
