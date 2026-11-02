using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddCustomer;
using TN.Account.Application.Account.Command.BillSundry;
using TN.Account.Application.Account.Command.UpdateBillSundry;
using TN.Account.Application.Account.Queries.FilterSundryBill;
using TN.Account.Application.Account.Queries.GetBillSundry;
using TN.Account.Application.Account.Queries.GetBillSundryById;
using TN.Shared.Application.Shared.Command.CalculationBillSundry;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.ServiceInterface
{
    public interface IBillSundryServices
    {
        Task<Result<AddBillSundryResponse>> Add(AddBillSundryCommand addBillSundryCommand);
 
        Task<Result<PagedResult<GetBillSundryQueryResponse>>> GetSundryBill(PaginationRequest paginationRequest,CancellationToken cancellationToken=default);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<GetBillSundryByIdQueryResponse>> GetSundryBillById(string Id,CancellationToken cancellationToken = default);

        Task<Result<CalculationBillResponseDTOs>> CalculateBillSundry(CalculationBIllDTOs addBillSundryRequest);
        Task<Result<PagedResult<FilterSundryBillQueryResponse>>> FilterBillSundry(PaginationRequest paginationRequest,FilterSundryBillDto filterSundryBillDto);
        Task<Result<UpdateBillSundryResponse>> Update(string Id, UpdateBillSundryCommand updateBillSundryCommand);

    }
}
