using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.SalesReturn.Command.AddSalesReturnDetails;
using TN.Sales.Application.SalesReturn.Command.UpdateSalesReturnDetails;
using TN.Sales.Application.SalesReturn.Queries;
using TN.Sales.Application.SalesReturn.Queries.FilterSalesReturnDetailsByDate;
using TN.Sales.Application.SalesReturn.Queries.GetAllSalesReturnItems;
using TN.Sales.Application.SalesReturn.Queries.GetSalesReturnDetailsById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Sales.Application.ServiceInterface
{
    public interface ISalesReturnServices
    {
        Task<Result<AddSalesReturnDetailsResponse>> Add(AddSalesReturnDetailsCommand request);
        Task<Result<PagedResult<GetAllSalesReturnDetailsByQueryResponse>>> GetAllSalesReturnDetails(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<GetSalesReturnDetailsByIdQueryResponse>> GetSalesReturnDetailsById(string id,CancellationToken cancellationToken = default);
        Task<Result<UpdateSalesReturnDetailsResponse>> Update(UpdateSalesReturnDetailsCommand request, string id);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<PagedResult<GetAllSalesReturnItemsByQueryResponse>>> GetAllSalesReturnItems(PaginationRequest paginationRequest,CancellationToken cancellationToken = default);
        Task<Result<PagedResult<GetSalesReturnDetailsFilterQueryResponse>>> SalesReturnDetailsFilters(PaginationRequest paginationRequest,FilterSalesReturnDetailsDTOs filterSalesReturnDetailsDTOs);
    }
}
