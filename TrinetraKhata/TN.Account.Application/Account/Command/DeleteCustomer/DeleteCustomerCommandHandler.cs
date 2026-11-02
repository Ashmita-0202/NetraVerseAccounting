using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.DeleteCustomer
{
    public class DeleteCustomerCommandHandler:IRequestHandler<DeleteCustomerCommand,Result<bool>>
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public DeleteCustomerCommandHandler(ICustomerService customerService,IMapper mapper)
        { 
            _customerService=customerService;
            _mapper=mapper;
        }

        public async Task<Result<bool>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteCustomer = await _customerService.Delete(request.id, cancellationToken);
                if (deleteCustomer is null)
                {
                    return Result<bool>.Failure("NotFound", "Customer not Found");
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
