using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Reports.Application.AccountBook.Queries.PurchaseRegister;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.AccountBook.Queries.JournalRegister
{
   public class JournalRegisterQueryHandler:IRequestHandler<JournalRegisterQuery,Result<PagedResult<JournalRegisterQueryResponse>>>
    {
        private readonly IAccountBookServices _accountBookServices;
        private readonly IMapper _mapper;

        public JournalRegisterQueryHandler(IAccountBookServices accountBookServices,IMapper mapper)
        {
            _accountBookServices=accountBookServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<JournalRegisterQueryResponse>>> Handle(JournalRegisterQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _accountBookServices.GetJournalRegisterByLedger(request.PaginationRequest, request.JournalRegisterDTOs);

                var journalRegisterResponse = _mapper.Map<PagedResult<JournalRegisterQueryResponse>>(result.Data);

                return Result<PagedResult<JournalRegisterQueryResponse>>.Success(journalRegisterResponse);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<JournalRegisterQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
