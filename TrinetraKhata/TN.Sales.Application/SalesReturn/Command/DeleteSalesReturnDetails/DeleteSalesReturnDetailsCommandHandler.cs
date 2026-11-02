using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
namespace TN.Sales.Application.SalesReturn.Command.DeleteSalesReturnDetails
{
    public class DeleteSalesReturnDetailsCommandHandler:IRequestHandler<DeleteSalesReturnDetailsCommand, Result<bool>>
    {
        private readonly ISalesReturnServices _salesReturnServices;
        private readonly IMapper _mapper;

        public DeleteSalesReturnDetailsCommandHandler(ISalesReturnServices salesReturnServices,IMapper mapper)
        {
              _salesReturnServices=salesReturnServices;
              _mapper=mapper;
        
        }

        public async Task<Result<bool>> Handle(DeleteSalesReturnDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteSalesReturnDetails = await _salesReturnServices.Delete(request.id, cancellationToken);
                if (deleteSalesReturnDetails is null)
                {
                    return Result<bool>.Failure("NotFound", "salesReturn Details not Found");
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
