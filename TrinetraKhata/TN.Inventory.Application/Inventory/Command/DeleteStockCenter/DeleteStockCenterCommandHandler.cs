using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.DeleteStockCenter
{
    public class DeleteStockCenterCommandHandler:IRequestHandler<DeleteStockCenterCommand,Result<bool>>
    {
        private readonly IStockCenterService _service;

        public DeleteStockCenterCommandHandler(IStockCenterService service)
        {
            _service = service;
        }

        public async Task<Result<bool>> Handle(DeleteStockCenterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteStockCenter = await _service.Delete(request.Id, cancellationToken);
                if (deleteStockCenter is null)
                {
                    return Result<bool>.Failure("NotFound", "StockCenter not Found");
                }
                return Result<bool>.Success(true);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.Id}", ex);
            }
        }
    }
}
