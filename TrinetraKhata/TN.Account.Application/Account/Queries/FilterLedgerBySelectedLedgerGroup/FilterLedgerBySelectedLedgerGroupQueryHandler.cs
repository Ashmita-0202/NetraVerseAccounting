using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.Customer;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.FilterLedgerBySelectedLedgerGroup
{
    public class FilterLedgerBySelectedLedgerGroupQueryHandler : IRequestHandler<FilterLedgerBySelectedLedgerGroupQuery, Result<IEnumerable<FilterLedgerBySelectedLedgerGroupResponse>>>
    {
        private readonly ILedgerGroupService _ledgerGroupService;
        private readonly IMapper _mapper;

        public FilterLedgerBySelectedLedgerGroupQueryHandler(ILedgerGroupService ledgerGroupService, IMapper mapper)
        {
            _mapper = mapper;
            _ledgerGroupService = ledgerGroupService;
            
        }

        public async Task<Result<IEnumerable<FilterLedgerBySelectedLedgerGroupResponse>>> Handle(FilterLedgerBySelectedLedgerGroupQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterLedger = await _ledgerGroupService.GetFilterLedger(cancellationToken);
                var filterLedgerResult = _mapper.Map<IEnumerable<FilterLedgerBySelectedLedgerGroupResponse>>(filterLedger.Data);
                return Result<IEnumerable<FilterLedgerBySelectedLedgerGroupResponse>>.Success(filterLedgerResult);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all customer", ex);
            }
        }
    }
}
