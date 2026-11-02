using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.DeleteLedgerGroup
{
    public class DeleteLedgerGroupCommandHandler : IRequestHandler<DeleteLedgerGroupCommand,Result<bool>>
    {
        private readonly ILedgerGroupService _ledgerGroupService;
        private readonly IMapper _mapper;

        public DeleteLedgerGroupCommandHandler(ILedgerGroupService ledgerGroupService,IMapper mapper) 
        {
            _ledgerGroupService=ledgerGroupService;
            _mapper=mapper;
        
        }

        public async Task<Result<bool>> Handle(DeleteLedgerGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteLedger = await _ledgerGroupService.Delete(request.id, cancellationToken);
                if (deleteLedger is null)
                {
                    return Result<bool>.Failure("NotFound", "LedgerGroup not Found");
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
