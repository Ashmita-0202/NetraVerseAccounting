using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.PurchaseReturn.Command.DeletePurchaseReturnDetails
{
    public class DeletePurchaseReturnDetailsCommandHandler:IRequestHandler<DeletePurchaseReturnDetailsCommand,Result<bool>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;
        private readonly IMapper _mapper;

        public DeletePurchaseReturnDetailsCommandHandler(IPurchaseDetailsServices purchaseDetailsServices,IMapper mapper)
        {
            _purchaseDetailsServices=purchaseDetailsServices;
            _mapper=mapper;
        }

        public async Task<Result<bool>> Handle(DeletePurchaseReturnDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deletePurchaseReturnDetails = await _purchaseDetailsServices.DeletePurchaseReturnDetails(request.id, cancellationToken);
                if (deletePurchaseReturnDetails is null)
                {
                    return Result<bool>.Failure("NotFound", "PurchaseReturn Details not Found");
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
