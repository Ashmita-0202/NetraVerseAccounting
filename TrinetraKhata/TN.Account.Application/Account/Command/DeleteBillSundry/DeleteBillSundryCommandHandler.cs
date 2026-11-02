using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.DeleteBillSundry
{
    public  class DeleteBillSundryCommandHandler: IRequestHandler<DeleteBillSundryCommand, Result<bool>>
    {
        private readonly IBillSundryServices _billSundryServices;
        private readonly IMapper _mapper;

        public DeleteBillSundryCommandHandler(IBillSundryServices billSundryServices,IMapper mapper)
        {
            _billSundryServices=billSundryServices;
            _mapper=mapper;
        }

        public async Task<Result<bool>> Handle(DeleteBillSundryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteBillSundry = await _billSundryServices.Delete(request.id, cancellationToken);
                if (deleteBillSundry is null)
                {
                    return Result<bool>.Failure("NotFound", "Bill Sundry not Found");
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
