using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.DeleteLedger
{
    public class DeleteLedgerCommandHandler :IRequestHandler<DeleteLedgerCommand,Result<bool>>
    {
        private readonly ILedgerService _ledgerServices;
        private readonly IMapper _mapper;

        public DeleteLedgerCommandHandler(ILedgerService ledgerService,IMapper mapper)
        {
          _ledgerServices=ledgerService;
            _mapper=mapper;
        
        }

        public async Task<Result<bool>> Handle(DeleteLedgerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteLedger = await _ledgerServices.Delete(request.id, cancellationToken);
                if (deleteLedger is null)
                {
                    return Result<bool>.Failure("NotFound", "Ledger not Found");
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
