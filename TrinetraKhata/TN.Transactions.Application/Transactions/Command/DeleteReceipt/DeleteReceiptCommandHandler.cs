using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;

namespace TN.Transactions.Application.Transactions.Command.DeleteReceipt
{
    public  class DeleteReceiptCommandHandler:IRequestHandler<DeleteReceiptCommand,Result<bool>>
    {
        private readonly IReceiptServices _receiptServices;

        public DeleteReceiptCommandHandler(IReceiptServices receiptServices) 
        {
            _receiptServices=receiptServices;
        }

        public async Task<Result<bool>> Handle(DeleteReceiptCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteReceipt = await _receiptServices.Delete(request.id, cancellationToken);
                if (deleteReceipt is null)
                {
                    return Result<bool>.Failure("NotFound", "receipt not Found");
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
