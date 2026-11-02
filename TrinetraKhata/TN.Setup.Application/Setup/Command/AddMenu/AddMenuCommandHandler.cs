using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AddMenu
{
    public class AddMenuCommandHandler : IRequestHandler<AddMenuCommand, Result<AddMenuResponse>>
    {
        private readonly IValidator<AddMenuCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IMenuServices _menuServices;

        public AddMenuCommandHandler(IValidator<AddMenuCommand> validator, IMapper mapper, IMenuServices menuServices)
        {
            _menuServices = menuServices;
            _validator = validator;
            _mapper = mapper;
            
        }
        public async Task<Result<AddMenuResponse>> Handle(AddMenuCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if(!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x=>x.ErrorMessage));
                    return Result<AddMenuResponse>.Failure(errors);
                }

                var addMenu = await _menuServices.Add(request);

                if(addMenu.Errors.Any())
                {
                    var errors = string.Join(", ", addMenu.Errors);
                    return Result<AddMenuResponse>.Failure(errors);
                }

                if(addMenu is null || !addMenu.IsSuccess)
                {
                    return Result<AddMenuResponse>.Failure("Add modules failed");
                }

                var menuDisplays = _mapper.Map<AddMenuResponse>(request);
                return Result<AddMenuResponse>.Success(menuDisplays);

            }catch (Exception ex)
            {
                throw new Exception("An error occurred while adding menu", ex);
            }
        }
    }
}
