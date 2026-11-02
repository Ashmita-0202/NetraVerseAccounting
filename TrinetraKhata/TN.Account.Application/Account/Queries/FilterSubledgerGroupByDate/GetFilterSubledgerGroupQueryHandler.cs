using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.Account.Queries.FilterLedgerGroup;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.FilterSubledgerGroupByDate
{
    public class GetFilterSubledgerGroupQueryHandler : IRequestHandler<GetFilterSubledgerGroupQuery, Result<PagedResult<GetFilterSubledgerGroupQueryResponse>>>
    {
        private readonly ISubledgerGroupService _subledgerGroupService;
        private readonly IMapper _mapper;

        public GetFilterSubledgerGroupQueryHandler(ISubledgerGroupService subledgerGroupService,IMapper mapper)
        {
            _subledgerGroupService = subledgerGroupService;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetFilterSubledgerGroupQueryResponse>>> Handle(GetFilterSubledgerGroupQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _subledgerGroupService.GetFilterSubLedgerGroup(request.PaginationRequest, request.FilterSubledgerGroupDto);

                var subLedgerGroupResult = _mapper.Map<PagedResult<GetFilterSubledgerGroupQueryResponse>>(result.Data);

                return Result<PagedResult<GetFilterSubledgerGroupQueryResponse>>.Success(subLedgerGroupResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetFilterSubledgerGroupQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
