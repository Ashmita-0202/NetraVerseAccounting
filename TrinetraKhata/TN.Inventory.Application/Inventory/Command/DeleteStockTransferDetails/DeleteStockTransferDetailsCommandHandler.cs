using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;


namespace TN.Account.Application.Account.Command.DeleteStockTransferDetails
{
    public class DeleteStockTransferDetailsCommandHandler:IRequestHandler<DeleteStockTransferDetailsCommand,Result<bool>>
    {
        private readonly IStockTransferDetailsServices _detailsServices;
        private readonly IMapper _mapper;

        public DeleteStockTransferDetailsCommandHandler(IStockTransferDetailsServices detailsServices,IMapper mapper)
        {
            _detailsServices=detailsServices;
            _mapper=mapper;

        }

        public async Task<Result<bool>> Handle(DeleteStockTransferDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteStocktransfer = await _detailsServices.Delete(request.id, cancellationToken);
                if (deleteStocktransfer is null)
                {
                    return Result<bool>.Failure("NotFound", "StockTransferDetails not Found");
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
