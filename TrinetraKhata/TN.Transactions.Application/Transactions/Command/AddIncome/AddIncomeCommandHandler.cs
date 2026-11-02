using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;
using TN.Transactions.Application.Transactions.Command.AddPayments;

namespace TN.Transactions.Application.Transactions.Command.AddIncome
{
    public class AddIncomeCommandHandler : IRequestHandler<AddIncomeCommand, Result<AddIncomeResponse>>
    {

        private readonly IIncomeService _incomeServices;
        private readonly IValidator<AddIncomeCommand> _validator;
        private readonly IMapper _mapper;

        public AddIncomeCommandHandler(IIncomeService incomeService, IValidator<AddIncomeCommand> validator)
        {
            _incomeServices = incomeService;
            _validator = validator;
            
        }


        public async Task<Result<AddIncomeResponse>> Handle(AddIncomeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddIncomeResponse>.Failure(errors);
                }

                var addIncome = await _incomeServices.Add(request);

                if (addIncome.Errors.Any())
                {
                    var errors = string.Join(", ", addIncome.Errors);
                    return Result<AddIncomeResponse>.Failure(errors);
                }

                if (addIncome is null || !addIncome.IsSuccess)
                {
                    return Result<AddIncomeResponse>.Failure(" ");
                }
                if (addIncome.Data == null)
                {
                    return Result<AddIncomeResponse>.Failure("No income data returned.");
                }

                //var addPaymentsResponse = _mapper.Map<AddPaymentsResponse>(addPayments.Data);

                return Result<AddIncomeResponse>.Success(addIncome.Data);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Income", ex);
            }
        }
    }
}
