using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.Account.Queries.GetMasterById;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.LedgerGroupById
{
    public sealed class GetLedgerGroupByIdQueryHandler : IRequestHandler<GetLedgerGroupByIdQuery, Result<GetLedgerGroupByIdResponse>>
    {
        private readonly ILedgerGroupService _service;
        private readonly IMapper _mapper;

        public GetLedgerGroupByIdQueryHandler(ILedgerGroupService ledgerGroupService,IMapper mapper)
        {
            _service=ledgerGroupService;
            _mapper=mapper;
        }

        public async Task<Result<GetLedgerGroupByIdResponse>> Handle(GetLedgerGroupByIdQuery request, CancellationToken cancellationToken)
        {
            try {

                var ledgerGroupById = await _service.GetLedgerGroupById(request.id);

                return Result<GetLedgerGroupByIdResponse>.Success(ledgerGroupById.Data);


            } catch (Exception ex)
            
            {

            throw new Exception("An error occurred while fetching Ledger Group by Id", ex);
            
            }
        }
    }
}
