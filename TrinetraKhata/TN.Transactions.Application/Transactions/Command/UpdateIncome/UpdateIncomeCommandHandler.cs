using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.receiptDatas.Application.receiptDatas.Command.UpdateReceipt;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;

namespace TN.Transactions.Application.Transactions.Command.UpdateIncome
{
    public class UpdateIncomeCommandHandler:IRequestHandler<UpdateIncomeCommand,Result<UpdateIncomeResponse>>
    {
        private readonly IIncomeService _incomeService;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateIncomeCommand> _validator;

        public UpdateIncomeCommandHandler(IIncomeService incomeService,IMapper mapper,IValidator<UpdateIncomeCommand> validator) 
        {
            _incomeService= incomeService;
            _mapper=mapper;
            _validator=validator;

        }

        public async Task<Result<UpdateIncomeResponse>> Handle(UpdateIncomeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateIncomeResponse>.Failure(errors);

                }

                var updateReceipt = await _incomeService.Update(request.id,request);

                if (updateReceipt.Errors.Any())
                {
                    var errors = string.Join(", ", updateReceipt.Errors);
                    return Result<UpdateIncomeResponse>.Failure(errors);
                }

                if (updateReceipt is null || !updateReceipt.IsSuccess)
                {
                    return Result<UpdateIncomeResponse>.Failure("Updates Transactions failed");
                }

                var receiptDisplay = _mapper.Map<UpdateIncomeResponse>(updateReceipt.Data);
                return Result<UpdateIncomeResponse>.Success(receiptDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating Income by {request.id}", ex);
            }
        }
    }
}
