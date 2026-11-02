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

namespace TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase
{
   public class UpdateBillNumberStatusForPurchaseCommandHandler: IRequestHandler<UpdateBillNumberStatusForPurchaseCommand, Result<UpdateBillNumberStatusForPurchaseResponse>>
    {
        private readonly ICompanyServices _companyServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateBillNumberStatusForPurchaseCommand> _validator;

        public UpdateBillNumberStatusForPurchaseCommandHandler(ICompanyServices companyServices, IMapper mapper,IValidator<UpdateBillNumberStatusForPurchaseCommand> validator)
        {
            _companyServices=companyServices;
            _mapper=mapper;
            _validator=validator;
            
        }

        public async  Task<Result<UpdateBillNumberStatusForPurchaseResponse>> Handle(UpdateBillNumberStatusForPurchaseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateBillNumberStatusForPurchaseResponse>.Failure(errors);

                }

                var updateBillNumber = await _companyServices.Update(request.id, request.billNumberGenerationTypeForPurchase);

                if (updateBillNumber.Errors.Any())
                {
                    var errors = string.Join(", ", updateBillNumber.Errors);
                    return Result<UpdateBillNumberStatusForPurchaseResponse>.Failure(errors);
                }

                if (updateBillNumber is null || !updateBillNumber.IsSuccess)
                {
                    return Result<UpdateBillNumberStatusForPurchaseResponse>.Failure("Updates bill number generation for purchase is  failed");
                }

                var companyDisplay = _mapper.Map<UpdateBillNumberStatusForPurchaseResponse>(updateBillNumber.Data);
                return Result<UpdateBillNumberStatusForPurchaseResponse>.Success(companyDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating bill number status", ex);

            }
        }
    }
}
