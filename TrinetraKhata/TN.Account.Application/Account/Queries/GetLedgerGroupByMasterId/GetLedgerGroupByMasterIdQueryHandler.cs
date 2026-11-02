using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.GetLedgerGroupByMasterId
{
    public class GetLedgerGroupByMasterIdQueryHandler : IRequestHandler<GetLedgerGroupByMasterIdQuery, Result<List<GetLedgerGroupByMasterIdResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ILedgerGroupService _service;

        public GetLedgerGroupByMasterIdQueryHandler(IMapper mapper, ILedgerGroupService ledgerGroupService)
        {
            _mapper = mapper;
            _service=ledgerGroupService;
            
        }
        public async Task<Result<List<GetLedgerGroupByMasterIdResponse>>> Handle(GetLedgerGroupByMasterIdQuery request, CancellationToken cancellationToken)
        {
            try {

                var ledgerGroupByMasterId = await _service.GetLedgerGroupByMasterId(request.masterId, cancellationToken);
                return Result<List<GetLedgerGroupByMasterIdResponse>>.Success(ledgerGroupByMasterId.Data);
            } catch (Exception ex) 
            
            {
             throw new Exception($"An error occurred while fetching Ledger Group By using {request.masterId}", ex);
            }
        }
    }
}
