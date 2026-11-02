using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.UpdateBillSundry
{
    public class UpdateBillSundryCommandHandler:IRequestHandler<UpdateBillSundryCommand, Result<UpdateBillSundryResponse>>
    {
        private readonly IBillSundryServices _billSundryServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateBillSundryCommand> _validator;

        public UpdateBillSundryCommandHandler(IBillSundryServices billSundryServices,IMapper mapper,IValidator<UpdateBillSundryCommand> validator)
        {
            _billSundryServices= billSundryServices;
            _mapper = mapper;
            _validator= validator;
        }

        public async Task<Result<UpdateBillSundryResponse>> Handle(UpdateBillSundryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateBillSundryResponse>.Failure(errors);

                }

                var updateBillSundry = await _billSundryServices.Update(request.Id, request);

                if (updateBillSundry.Errors.Any())
                {
                    var errors = string.Join(", ", updateBillSundry.Errors);
                    return Result<UpdateBillSundryResponse>.Failure(errors);
                }

                if (updateBillSundry is null || !updateBillSundry.IsSuccess)
                {
                    return Result<UpdateBillSundryResponse>.Failure("Updates Bill sundry failed");
                }

                var display = _mapper.Map<UpdateBillSundryResponse>(updateBillSundry.Data);
                return Result<UpdateBillSundryResponse>.Success(display);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating Bill sundry by {request.id}", ex);
            }
        }
    }
}
