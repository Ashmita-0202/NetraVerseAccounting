using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.CloseFiscalYear
{
    public class CloseFiscalYearCommandHandler : IRequestHandler<CloseFiscalYearCommand, Result<CloseFiscalYearResponse>>
    {
        private readonly IFiscalYearService _faiscalYearService;
        private readonly IMapper _mapper;
        private readonly IValidator<CloseFiscalYearCommand> _validator;



        public CloseFiscalYearCommandHandler(IFiscalYearService fiscalYearService, IMapper mapper, IValidator<CloseFiscalYearCommand> validator)
        {
            _faiscalYearService = fiscalYearService;
            _mapper = mapper;
            _validator = validator;
            
        }
        public async Task<Result<CloseFiscalYearResponse>> Handle(CloseFiscalYearCommand request, CancellationToken cancellationToken)
        {

                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<CloseFiscalYearResponse>.Failure(errors);
                }

                var closedFiscalYear = await _faiscalYearService.CloseFiscalYear(request);

                if (closedFiscalYear.Errors.Any())
                {
                    var errors = string.Join(", ", closedFiscalYear.Errors);
                    return Result<CloseFiscalYearResponse>.Failure(errors);
                }

                if (closedFiscalYear is null || !closedFiscalYear.IsSuccess)
                {
                    return Result<CloseFiscalYearResponse>.Failure(" ");
                }

                var closedFiscalYearDisplay = _mapper.Map<CloseFiscalYearResponse>(closedFiscalYear.Data);
                return Result<CloseFiscalYearResponse>.Success(closedFiscalYearDisplay);


            

        }
    }
}
