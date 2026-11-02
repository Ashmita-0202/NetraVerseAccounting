using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Command.AddPurchaseDetails;
using TN.Purchase.Application.Purchase.Command.QuotationToPurchase;
using TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationByCompany;
using TN.Purchase.Application.Purchase.Command.UpdatePurchaseDetails;
using TN.Purchase.Application.Purchase.Queries.BillNumberGenerationByCompany;
using TN.Purchase.Application.Purchase.Queries.FilterPurchaseDetailsByDate;
using TN.Purchase.Application.Purchase.Queries.FilterPurchaseQuotationByDate;
using TN.Purchase.Application.Purchase.Queries.GetPurchaseDetailsByRefNo;
using TN.Purchase.Application.Purchase.Queries.GetPurchaseQuotationById;
using TN.Purchase.Application.Purchase.Queries.Purchase;
using TN.Purchase.Application.Purchase.Queries.PurchaseDetailsById;
using TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnDetails;
using TN.Purchase.Application.PurchaseReturn.Command.UpdatePurchaseReturnDetails;
using TN.Purchase.Application.PurchaseReturn.Queries.AllPurchaseReturnDetails;
using TN.Purchase.Application.PurchaseReturn.Queries.FilterPurchaseReturnDetailsByDate;
using TN.Purchase.Application.PurchaseReturn.Queries.PurchaseReturnDetailsById;
using TN.Sales.Application.Sales.Command.QuotationToSales;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Purchase.Application.ServiceInterface
{
    public interface IPurchaseDetailsServices
    {
        Task<Result<AddPurchaseDetailsResponse>> Add(AddPurchaseDetailsCommand request);
        Task<Result<QuotationToPurchaseResponse>> QuotationToPurchase(QuotationToPurchaseCommand quotationToPurchaseCommand);
        Task<Result<string>> GetCurrentPurchaseBillNumber();
        Task<Result<string>> GetCurrentPurchaseReferenceNumber();
        Task<Result<AddPurchaseReturnDetailsResponse>> AddPurchaseReturn(AddPurchaseReturnDetailsCommand request);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<PagedResult<FilterPurchaseDetailsByDateQueryResponse>>> GetPurchaseDetailsFilter(PaginationRequest paginationRequest,FilterPurchaseDetailsDTOs filterPurchaseDetailsDTOs);
        Task<Result<PagedResult<FilterPurchaseQuotationQueryResponse>>> GetPurchaseQuotationFilter(PaginationRequest paginationRequest, FilterPurchaseDetailsDTOs filterPurchaseDetailsDTOs);
        Task<Result<BillNumberGenerationByCompanyQueryResponse>> GetBillNumberStatusByCompany(string id,CancellationToken cancellationToken);
        Task<Result<GetPurchaseDetailsByIdQueryResponse>> GetPurchaseDetailsById(string id,CancellationToken cancellationToken=default);
      Task<Result<GetPurchaseQuotationByIdQueryResponse>> GetPurchaseQuotationById(string id, CancellationToken cancellationToken = default);

        Task<Result<PurchaseReturnDetailsByIdQueryResponse>> GetPurchaseReturnDetailsById(string id, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<GetAllPurchaseDetailsQueryResponse>>> GetAllPurchaseDetails(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<PurchaseReturnDetailsQueryResponse>>> GetAllPurchaseReturnDetails(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<UpdatePurchaseDetailsResponse>> Update(string id, UpdatePurchaseDetailsCommand updatePurchaseDetailsCommand);
        Task<Result<UpdateBillNumberGeneratorByCompanyResponse>> UpdateBillNumberStatusByCompany(UpdateBillNumberGeneratorByCompanyCommand updateBillNumberGeneratorByCompanyCommand);
        Task<Result<bool>> DeletePurchaseReturnDetails(string id, CancellationToken cancellationToken);
        Task<Result<UpdatePurchaseReturnDetailsResponse>> Update(string id, UpdatePurchaseReturnDetailsCommand updatePurchaseReturnDetailsCommand);
   
        Task<Result<GetPurchaseDetailsQueryResponse>> GetPurchaseDetailsByRefNo(string referenceNumber, CancellationToken cancellationToken=default);
        Task<Result<PagedResult<GetFilterPurchaseReturnDetailsQueryResponse>>> GetFilterPurchaseReturnDetails(PaginationRequest paginationRequest,FilterPurchaseReturnDetailsDtos filterPurchaseReturnDetailsDtos);
    

    }
}