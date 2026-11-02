using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddLedger;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.BillSundry
{
    public class AddBillSundryCommandHandler : IRequestHandler<AddBillSundryCommand, Result<AddBillSundryResponse>>
    {
        private readonly IBillSundryServices _billSundryServices;
        private readonly IValidator<AddBillSundryCommand> _validator;
        private readonly IMapper _mapper;

        public AddBillSundryCommandHandler(IBillSundryServices billSundryServices,IValidator<AddBillSundryCommand> validator, IMapper mapper)
        {
            _billSundryServices = billSundryServices;
            _validator = validator;
            _mapper = mapper;

        }


        public async Task<Result<AddBillSundryResponse>> Handle(AddBillSundryCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddBillSundryResponse>.Failure(errors);
                }

                var addBillSundry = await _billSundryServices.Add(request);

                if (addBillSundry.Errors.Any())
                {
                    var errors = string.Join(", ", addBillSundry.Errors);
                    return Result<AddBillSundryResponse>.Failure(errors);
                }

                if (addBillSundry is null || !addBillSundry.IsSuccess)
                {
                    return Result<AddBillSundryResponse>.Failure(" ");
                }

                var billSundryDisplays = _mapper.Map<AddBillSundryResponse>(addBillSundry.Data);
                return Result<AddBillSundryResponse>.Success(billSundryDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Bill Sundry", ex);


            }
        }
    }
}
