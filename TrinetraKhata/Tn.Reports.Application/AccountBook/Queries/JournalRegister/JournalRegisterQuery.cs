using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.AccountBook.Queries.JournalRegister
{
   public record JournalRegisterQuery
   (PaginationRequest PaginationRequest, JournalRegisterDTOs JournalRegisterDTOs):IRequest<Result<PagedResult<JournalRegisterQueryResponse>>>;
}
