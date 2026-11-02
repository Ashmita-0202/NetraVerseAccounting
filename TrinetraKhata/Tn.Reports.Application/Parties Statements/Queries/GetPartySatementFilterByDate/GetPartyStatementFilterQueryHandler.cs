using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Reports.Application.Parties_Statements.Queries.GetPartySatementFilter;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.Parties_Statements.Queries.GetPartySatementFilterByDate
{
    public class GetPartyStatementFilterQueryHandler: IRequestHandler<GetPartyStatementFilterQuery, Result<PagedResult<GetPartyStatementFilterResponse>>>
    {
        private readonly IPartyStatementServices _services;
        private readonly IMapper _mapper;

        public GetPartyStatementFilterQueryHandler(IPartyStatementServices services,IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetPartyStatementFilterResponse>>> Handle(GetPartyStatementFilterQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var filterPurchaseDetails = await _services.GetPartyStatementFilter(request.PaginationRequest, request.PartyStatementDto);


                if (!filterPurchaseDetails.IsSuccess || filterPurchaseDetails.Data == null)
                {
                    return Result<PagedResult<GetPartyStatementFilterResponse>>.Failure(filterPurchaseDetails.Message);
                }

                var filterPurchaseDetailsResult = _mapper.Map<PagedResult<GetPartyStatementFilterResponse>>(filterPurchaseDetails.Data);

                return Result<PagedResult<GetPartyStatementFilterResponse>>.Success(filterPurchaseDetailsResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetPartyStatementFilterResponse>>.Failure(
                    $"An error occurred while fetching PartyStatement  by date: {ex.Message}");
            }
        }
    }
    
}
