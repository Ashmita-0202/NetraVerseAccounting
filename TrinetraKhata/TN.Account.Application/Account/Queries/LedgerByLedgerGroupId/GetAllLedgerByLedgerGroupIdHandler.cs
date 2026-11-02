using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.LedgerGroup;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.LedgerByLedgerGroupId
{
    public sealed class GetAllLedgerByLedgerGroupIdHandler : IRequestHandler<GetAllLedgerByLedgerGroupIdQuery, Result<List<GetAllLedgerByLedgerGroupIdResponse>>>
    {
        private readonly ILedgerService _service;
        private readonly IMapper _mapper;

        public GetAllLedgerByLedgerGroupIdHandler(ILedgerService ledgerService, IMapper mapper)
        {
            _service = ledgerService;
            _mapper = mapper;
            
        }
        public async Task<Result<List<GetAllLedgerByLedgerGroupIdResponse>>> Handle(GetAllLedgerByLedgerGroupIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allLedger = await _service.GetLedgerByLedgerGroupId(request.ledgerGroupId, cancellationToken);
                var allLedgerDisplay = _mapper.Map<List<GetAllLedgerByLedgerGroupIdResponse>>(allLedger.Data);
                return Result<List<GetAllLedgerByLedgerGroupIdResponse>>.Success(allLedgerDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception($"An error occurred while fetching all ledger by LedgerGroupId {request.ledgerGroupId}", ex);
            }
        }
    }
}
