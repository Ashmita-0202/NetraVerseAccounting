using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Command.AddPurchaseDetails;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationByCompany
{
    public class UpdateBillNumberGeneratorByCompanyCommandHandler : IRequestHandler<UpdateBillNumberGeneratorByCompanyCommand, Result<UpdateBillNumberGeneratorByCompanyResponse>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateBillNumberGeneratorByCompanyCommand> _validator;

        public UpdateBillNumberGeneratorByCompanyCommandHandler(IPurchaseDetailsServices purchaseDetailsServices, IMapper mapper, IValidator<UpdateBillNumberGeneratorByCompanyCommand> validator)
        {
            _purchaseDetailsServices = purchaseDetailsServices;
            _mapper = mapper;
            _validator = validator;

        }
        public async Task<Result<UpdateBillNumberGeneratorByCompanyResponse>> Handle(UpdateBillNumberGeneratorByCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateBillNumberGeneratorByCompanyResponse>.Failure(errors);
                }

                var updateBillNumberStatusByCompany = await _purchaseDetailsServices.UpdateBillNumberStatusByCompany(request);

                if (updateBillNumberStatusByCompany.Errors.Any())
                {
                    var errors = string.Join(", ", updateBillNumberStatusByCompany.Errors);
                    return Result<UpdateBillNumberGeneratorByCompanyResponse>.Failure(errors);
                }

                if (updateBillNumberStatusByCompany is null || !updateBillNumberStatusByCompany.IsSuccess)
                {
                    return Result<UpdateBillNumberGeneratorByCompanyResponse>.Failure(" ");
                }
                var updateBillNumberGeneratorResponse = _mapper.Map<UpdateBillNumberGeneratorByCompanyResponse>(updateBillNumberStatusByCompany.Data);
                return Result<UpdateBillNumberGeneratorByCompanyResponse>.Success(updateBillNumberGeneratorResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding purchaseDetails", ex);
            }
        }
    }
}
