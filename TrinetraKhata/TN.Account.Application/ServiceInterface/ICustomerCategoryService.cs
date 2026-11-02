using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddCustomer;
using TN.Account.Application.Account.Command.AddCustomerCategory;
using TN.Account.Application.Account.Command.UpdateCustomer;
using TN.Account.Application.Account.Command.UpdateCustomerCategory;
using TN.Account.Application.Account.Queries.Customer;
using TN.Account.Application.Account.Queries.CustomerById;
using TN.Account.Application.Account.Queries.CustomerCategory;
using TN.Account.Application.Account.Queries.CustomerCategoryById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.ServiceInterface
{
    public interface ICustomerCategoryService
    {
        Task<Result<PagedResult<GetAllCustomerCategoryByResponse>>> GetAllCustomerCategory(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<GetCustomerCategoryByIdResponse>> GetCustomerCategoryById(string id, CancellationToken cancellationToken = default);
        Task<Result<AddCustomerCategoryResponse>> Add(AddCustomerCategoryCommand addCustomerCategoryCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<UpdateCustomerCategoryResponse>> Update(string id, UpdateCustomerCategoryCommand updateCustomerCategoryCommand);

    }
}
