using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
namespace TN.Sales.Application.Sales.Command.DeleteSalesDetails
{
public class DeleteSalesDetailsCommandHandler:IRequestHandler<DeleteSalesDetailsCommand, Result<bool>>
    {
        private readonly ISalesDetailsServices _salesDetailsServices;
        private readonly IMapper _mapper;

        public DeleteSalesDetailsCommandHandler(ISalesDetailsServices salesDetailsServices,IMapper mapper) 
        { 
            _salesDetailsServices=salesDetailsServices;
            _mapper=mapper;
        }

        public async Task<Result<bool>> Handle(DeleteSalesDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteSalesDetails = await _salesDetailsServices.Delete(request.id, cancellationToken);
                if (deleteSalesDetails is null)
                {
                    return Result<bool>.Failure("NotFound", "salesDetails not Found");
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
