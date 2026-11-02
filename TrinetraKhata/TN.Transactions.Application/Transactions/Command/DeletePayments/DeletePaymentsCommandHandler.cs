using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;

namespace TN.Transactions.Application.Transactions.Command.DeletePayment
{
    public class DeletePaymentsCommandHandler : IRequestHandler<DeletePaymentsCommand, Result<bool>>
    {
        private readonly IPaymentsServices _paymentsServices;
        private readonly IMapper _mapper;

        public DeletePaymentsCommandHandler(IPaymentsServices paymentsServices,IMapper mapper)
        {
            _paymentsServices = paymentsServices;
            _mapper = mapper;

        }
        public async Task<Result<bool>> Handle(DeletePaymentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deletePayment = await _paymentsServices.Delete(request.id, cancellationToken);
                if (deletePayment is null)
                {
                    return Result<bool>.Failure("NotFound", "payment not Found");
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