using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.DeleteStockAdjustment
{
    public class DeleteStockAdjustmentHandler:IRequestHandler<DeleteStockAdjustmentCommand, Result<bool>>
    {
        private readonly IInventoriesServices _services;

        public DeleteStockAdjustmentHandler(IInventoriesServices services)
        {
            _services = services;
            
            
        }

        public async Task<Result<bool>> Handle(DeleteStockAdjustmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteStockAdjustment = await _services.Delete(request.id,cancellationToken);
                if (deleteStockAdjustment is null)
                {
                    return Result<bool>.Failure("NotFound", "stock adjustment not Found");
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
