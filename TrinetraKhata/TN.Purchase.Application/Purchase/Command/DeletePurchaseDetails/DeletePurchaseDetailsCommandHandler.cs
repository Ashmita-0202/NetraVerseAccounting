using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Command.DeletePurchaseDetails
{
    public class DeletePurchaseDetailsCommandHandler:IRequestHandler<DeletePurchaseDetailsCommand, Result<bool>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;
        private readonly IMapper _mapper;

        public DeletePurchaseDetailsCommandHandler(IPurchaseDetailsServices purchaseDetailsServices,IMapper mapper)
        {
          _purchaseDetailsServices=purchaseDetailsServices;
            _mapper=mapper;
        
        }

        public async Task<Result<bool>> Handle(DeletePurchaseDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deletePurchaseDetails = await _purchaseDetailsServices.Delete(request.id, cancellationToken);
                if (deletePurchaseDetails is null)
                {
                    return Result<bool>.Failure("NotFound", "PurchaseDetails not Found");
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.id}", ex);
            }
        }
    }
}
