using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NV.Payment.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace NV.Payment.Application.Payment.Command.DeletePayment
{
    public class DeletePaymentCommandHandler:IRequestHandler<DeletePaymentCommand,Result<bool>>
    {
        private readonly IPaymentMethodService _service;
        private readonly IMapper _mapper;

        public DeletePaymentCommandHandler(IPaymentMethodService service,IMapper mapper) 
        {
            _service=service;
            _mapper=mapper;
        
        }

        public async Task<Result<bool>> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deletePaymentMethod = await _service.Delete(request.id, cancellationToken);
                if (deletePaymentMethod is null)
                {
                    return Result<bool>.Failure("NotFound", "Payment Method not Found");
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
