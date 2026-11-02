using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;


namespace TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationByCompany
{
    public class UpdateBillNumberGenerationByCompanyCommandHandler : IRequestHandler<UpdateBillNumberGenerationByCompanyCommand, Result<UpdateBillNumberGenerationByCompanyResponse>>
    {
        private readonly ISalesDetailsServices _salesDetailsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateBillNumberGenerationByCompanyCommand> _validator;

        public UpdateBillNumberGenerationByCompanyCommandHandler(ISalesDetailsServices salesDetailsServices, IMapper mapper, IValidator<UpdateBillNumberGenerationByCompanyCommand> validator)
        {
            _salesDetailsServices=salesDetailsServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<UpdateBillNumberGenerationByCompanyResponse>> Handle(UpdateBillNumberGenerationByCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateBillNumberGenerationByCompanyResponse>.Failure(errors);
                }

                var updateBillNumberStatusByCompany = await _salesDetailsServices.UpdateBillNumberStatusByCompany(request);

                if (updateBillNumberStatusByCompany.Errors.Any())
                {
                    var errors = string.Join(", ", updateBillNumberStatusByCompany.Errors);
                    return Result<UpdateBillNumberGenerationByCompanyResponse>.Failure(errors);
                }

                if (updateBillNumberStatusByCompany is null || !updateBillNumberStatusByCompany.IsSuccess)
                {
                    return Result<UpdateBillNumberGenerationByCompanyResponse>.Failure(" ");
                }
                var updateBillNumberGeneratorResponse = _mapper.Map<UpdateBillNumberGenerationByCompanyResponse>(updateBillNumberStatusByCompany.Data);
                return Result<UpdateBillNumberGenerationByCompanyResponse>.Success(updateBillNumberGeneratorResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding sales details", ex);
            }
        }
    }
}
