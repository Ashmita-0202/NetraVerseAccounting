using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.AccountBook.Queries.JournalRegister;
using TN.Reports.Application.AccountBook.Queries.PurchaseRegister;
using TN.Reports.Application.AccountBook.Queries.SalesRegister;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ServiceInterface
{
    public interface IAccountBookServices
    {
        Task<Result<PagedResult<SalesRegisterQueryResponse>>> GetSalesRegister(PaginationRequest paginationRequest,SalesRegisterDTOs salesRegisterDTOs);
        Task<Result<PagedResult<PurchaseRegisterQueryResponse>>> GetPurchaseRegister(PaginationRequest paginationRequest, PurchaseRegisterDTOs purchaseRegisterDTOs);
  
    Task<Result<PagedResult<JournalRegisterQueryResponse>>> GetJournalRegisterByLedger(PaginationRequest paginationRequest,JournalRegisterDTOs journalRegisterDTOs);
    
    }
}
