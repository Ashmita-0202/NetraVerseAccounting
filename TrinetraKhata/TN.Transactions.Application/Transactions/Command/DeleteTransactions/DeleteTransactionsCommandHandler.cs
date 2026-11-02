using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;

namespace TN.Transactions.Application.Transactions.Command.DeleteTransactions
{
    public  class DeleteTransactionsCommandHandler:IRequestHandler<DeleteTransactionsCommand, Result<bool>>
    {
        private readonly ITransactionsService _service;
        private readonly IMapper _mapper;

        public DeleteTransactionsCommandHandler(ITransactionsService service,IMapper mapper) 
        {
            _service=service;
            _mapper=mapper;
        
        }

        public async Task<Result<bool>> Handle(DeleteTransactionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteTransactions = await _service.Delete(request.id, cancellationToken);
                if (deleteTransactions is null)
                {
                    return Result<bool>.Failure("NotFound", "Transactions not Found");
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
