using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.Sales.Command.AddSalesDetails;
using TN.Sales.Application.Sales.Command.QuotationToSales;
using TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationByCompany;
using TN.Sales.Application.Sales.Command.UpdateSalesDetails;
using TN.Sales.Application.Sales.Queries.AllSalesDetails;
using TN.Sales.Application.Sales.Queries.BillNumberGenerationByCompany;
using TN.Sales.Application.Sales.Queries.FilterSalesDetailsByDate;
using TN.Sales.Application.Sales.Queries.FilterSalesQuotationByDate;
using TN.Sales.Application.Sales.Queries.GetAllSalesItems;
using TN.Sales.Application.Sales.Queries.GetSalesDetailsByRefNo;
using TN.Sales.Application.Sales.Queries.GetSalesQuotationById;
using TN.Sales.Application.Sales.Queries.SalesDetailsById;
using TN.Sales.Application.Sales.Queries.SalesItemByItemId;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Sales.Application.ServiceInterface
{
    public interface ISalesDetailsServices
    {
        Task<Result<AddSalesDetailsResponse>> Add(AddSalesDetailsCommand request);
        Task<Result<string>> GetCurrentSalesBillNumber();
        Task<Result<QuotationToSalesResponse>> QuotationToSales(QuotationToSalesCommand quotationToSalesCommand);
        Task<Result<UpdateSalesDetailsResponse>> Update(string id, UpdateSalesDetailsCommand updateSalesDetailsCommand);
        Task<Result<PagedResult<GetAllSalesDetailsByQueryResponse>>> GetAllSalesDetails(PaginationRequest paginationRequest, CancellationToken cancellationToken=default);
        Task<Result<BIllNumberGenerationByCompanyQueryResponse>> GetBillNumberStatusByCompany(string id, CancellationToken cancellationToken);
        Task<Result<GetSalesDetailsByIdQueryResponse>> GetSalesDetailsById(string id, CancellationToken cancellationToken = default);

        Task<Result<GetSalesItemsDetailsByItemIdQueryResponse>> GetSalesDetailsItems(string itemsId, CancellationToken cancellationToken = default);
        Task<Result<UpdateBillNumberGenerationByCompanyResponse>> UpdateBillNumberStatusByCompany(UpdateBillNumberGenerationByCompanyCommand updateBillNumberGenerationByCompanyCommand);
        Task<Result<PagedResult<FilterSalesDetailsByDateQueryResponse>>> GetFilterSalesDetails(PaginationRequest paginationRequest,FilterSalesDetailsDTOs filterSalesDetailsDTOs);

        Task<Result<PagedResult<FilterSalesQuotationQueryResponse>>> GetFilterSalesQuotation(PaginationRequest paginationRequest, FilterSalesDetailsDTOs filterSalesDetailsDTOs);


        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<PagedResult<GetAllSalesItemsByQueryResponse>>> GetAllSalesItems(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);

        Task<Result<GetSalesDetailsQueryResponse>> GetSalesDetailsByRefNo(string referenceNumber,CancellationToken cancellationToken=default);
    
        Task<Result<GetSalesQuotationByIdQueryResponse>> GetSalesQuotationById(string id, CancellationToken cancellationToken = default);

    }
}
