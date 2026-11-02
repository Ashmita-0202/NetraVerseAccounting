using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddCustomer;
using TN.Account.Application.Account.Command.AddLedger;
using TN.Account.Application.Account.Command.UpdateCustomer;
using TN.Account.Application.Account.Queries.Customer;
using TN.Account.Application.Account.Queries.CustomerById;
using TN.Account.Application.Account.Queries.LedgerById;
using TN.Account.Application.Account.Queries.LedgerGroup;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.ServiceInterface
{
    public interface ICustomerService
    {
        Task<Result<PagedResult<GetAllCustomerByQueryResponse>>> GetAllCustomer(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<GetCustomerByIdResponse>> GetCustomerById(string id, CancellationToken cancellationToken = default);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<AddCustomerResponse>> Add(AddCustomerCommand addCustomerCommand);
        Task<Result<UpdateCustomerResponse>> Update(string id, UpdateCustomerCommand updateCustomerCommand);
    }
}
