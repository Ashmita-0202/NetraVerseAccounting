using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.LedgerById
{
    public sealed class GetLedgerByIdQueryHandler : IRequestHandler<GetLedgerByIdQuery, Result<GetLedgerByIdQueryResponse>>
    {
        private readonly ILedgerService _service;
        private readonly IMapper _mapper;

        public GetLedgerByIdQueryHandler(ILedgerService ledgerService, IMapper mapper)
        {
            _service=ledgerService;
            _mapper=mapper;
        }
        public async Task<Result<GetLedgerByIdQueryResponse>> Handle(GetLedgerByIdQuery request, CancellationToken cancellationToken)
        {
            try {

                var ledgerById = await _service.GetLedgerById(request.id);
                return Result<GetLedgerByIdQueryResponse>.Success(ledgerById.Data);
            
            } catch (Exception ex) 
            {
                throw new Exception("An error occurred while fetching ledger by using id", ex);
            }
        }
    }
}
