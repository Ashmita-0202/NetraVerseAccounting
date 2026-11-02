
using TN.Authentication.Application.Authentication.Queries.GetExpiredDateItemStatusByCompany;
using TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear;
using TN.Shared.Application.Shared.Command.UpdateExpenseTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdateIncomeTransactionNumberTypeCommand;
using TN.Shared.Application.Shared.Command.UpdateInventoryMethodByCompany;
using TN.Shared.Application.Shared.Command.UpdateItemStatusByCompany;
using TN.Shared.Application.Shared.Command.UpdateJournalRefByCompany;
using TN.Shared.Application.Shared.Command.UpdatePaymentTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdatePurchaseRefNumberByCompany;
using TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType;
using TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdateSalesQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberByCompany;
using TN.Shared.Application.Shared.Command.UpdateSalesReturnType;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInPurchase;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInSales;
using TN.Shared.Application.Shared.Queries.GetAllCurrentFiscalYear;
using TN.Shared.Application.Shared.Queries.GetCurrentFiscalYear;
using TN.Shared.Application.Shared.Queries.GetExpenseTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetIncomeTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetInventoryMethodByCompany;
using TN.Shared.Application.Shared.Queries.GetJournalRefByCompany;
using TN.Shared.Application.Shared.Queries.GetPaymentTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetPurchaseQuotationNumber;
using TN.Shared.Application.Shared.Queries.GetPurchaseReferenceNumber;
using TN.Shared.Application.Shared.Queries.GetPurchaseReturnNumber;
using TN.Shared.Application.Shared.Queries.GetReceiptTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetSalesQuotationNumberType;
using TN.Shared.Application.Shared.Queries.GetSalesReferenceNumber;
using TN.Shared.Application.Shared.Queries.GetSalesReturnNumber;
using TN.Shared.Application.Shared.Queries.GetSerialNumberForPurchase;
using TN.Shared.Application.Shared.Queries.GetTaxStatusInPurchase;
using TN.Shared.Application.Shared.Queries.GetTaxStatusInSales;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using static TN.Authentication.Domain.Entities.CompanySettings;

namespace TN.Shared.Application.ServiceInterface
{
    public interface ISettingServices
    {
        Task<Result<PagedResult<GetAllFiscalYearQueryResponse>>> GetAllFiscalYear(PaginationRequest paginationRequest,CancellationToken cancellationToken=default);
       
        Task<Result<UpdateFiscalYearResponse>> UpdateFiscalYear(string companyId,string? currentFiscalYearId, CancellationToken cancellationToken);
        Task<Result<GetTaxStatusInPurchaseResponse>> GetTaxStatusInPurchase(string companyId, CancellationToken cancellationToken);
        Task<Result<GetTaxStatusInSalesResponse>> GetTaxStatusInSales(string companyId, CancellationToken cancellationToken);
        Task<Result<GetPurchaseReferenceNumberQueryResponse>> GetPurchaseReferenceNumber(string companyId, CancellationToken cancellationToken);
        Task<Result<GetSalesReferenceNumberQueryResponse>> GetSalesReferenceNumber(string companyId, CancellationToken cancellationToken);
        Task<Result<GetItemStatusByCompanyResponse>> GetItemStatusByCompany(string companyId,CancellationToken cancellationToken);
          Task<Result<UpdateItemStatusByCompanyResponse>> UpdateItemStatusByCompany(string companyId, bool isExpiredDate, bool isSerialNo, CancellationToken cancellationToken);
          Task<Result<GetJournalRefByCompanyQueryResponse>> GetJournalRefByCompany(string companyId, CancellationToken cancellationToken);
       
          Task<Result<UpdateJournalRefByCompanyResponse>> UpdateJournalRefByCompany(string companyId, JournalReferencesType journalReferences, CancellationToken cancellationToken);
          Task<Result<GetInventoryByCompanyIdQueryResponse>> GetInventoryMethodByCompany(string companyId, CancellationToken cancellationToken);
    
        Task<Result<GetSerialNumberForPurchaseQueryResponse>> GetSerialNumberForPurchase(string companyId,  CancellationToken cancellationToken);
         Task<Result<UpdateInventoryMethodResponse>> UpdateInventoryMethodByCompany(string companyId, InventoryMethodType inventoryMethod, CancellationToken cancellationToken);
           Task<Result<UpdateSalesReferenceNumberResponse>> UpdateSalesReferenceNumberByCompany(string companyId, bool showReferenceNumberForSales, CancellationToken cancellationToken); 
        Task<Result<UpdatePurchaseReferenceNumberResponse>> UpdatePurchaseReferenceNumberByCompany(string companyId, PurchaseReferencesType purchaseReferencesType, CancellationToken cancellationToken);
   
    Task<Result<UpdateTaxStatusInPurchaseResponse>> UpdateTaxStatusInPurchaseBy(string companyId, bool showTaxInPurchase, CancellationToken cancellationToken);

        Task<Result<UpdateTaxStatusInSalesResponse>> UpdateTaxStatusInSalesBy(string companyId, bool showTaxInSales, CancellationToken cancellationToken);

        Task<Result<GetCurrentFiscalYearQueryResponse>> GetCurrentFiscalYearBy(string companyId,  CancellationToken cancellationToken);

        Task<Result<ReceiptTransactionNumberTypeResponse>> GetReceiptTransactionType(string companyId, CancellationToken cancellationToken);
        Task<Result<GetPaymentTransactionNumberTypeResponse>> GetPaymentTransactionType(string companyId, CancellationToken cancellationToken);
        Task<Result<UpdateReceiptTransactionNumberTypeResponse>> UpdateReceiptTransactionType(string companyId, TransactionNumberType  transactionNumberType, CancellationToken cancellationToken);
        Task<Result<UpdatePaymentTransactionNumberTypeResponse>> UpdatePaymentTransactionType(string companyId, TransactionNumberType transactionNumberType, CancellationToken cancellationToken);

        Task<Result<GetIncomeTransactionNumberTypeResponse>> GetIncomeTransactionType(string companyId, CancellationToken cancellationToken);
        Task<Result<GetExpenseTransactionNumberTypeResponse>> GetExpenseTransactionType(string companyId, CancellationToken cancellationToken);

        Task<Result<UpdateExpenseTransactionNumberTypeResponse>> UpdateExpenseTransactionType(string companyId, TransactionNumberType transactionNumberType, CancellationToken cancellationToken);

        Task<Result<UpdateIncomeTransactionNumberTypeResponse>> UpdateIncomeTransactionType(string companyId, TransactionNumberType transactionNumberType, CancellationToken cancellationToken);
   
        Task<Result<GetPurchaseReturnNumberQueryResponse>> GetPurchaseReturnNumber(string companyId,CancellationToken cancellationToken);
       
        Task<Result<GetSalesReturnNumberQueryResponse>> GetSalesReturnNumber(string companyId, CancellationToken cancellationToken);
        Task<Result<UpdatePurchaseReturnTypeResponse>> UpdatePurchaseReturnType(string companyId, PurchaseSalesReturnNumberType purchaseReturnNumberType, CancellationToken cancellationToken);

        Task<Result<UpdateSalesReturnTypeResponse>> UpdateSalesReturnType(string companyId, PurchaseSalesReturnNumberType salesReturnNumberType, CancellationToken cancellationToken);
        Task<Result<GetPurchaseQuotationNumberQueryResponse>> GetPurchaseQuotationNumber(string companyId, CancellationToken cancellationToken);

        Task<Result<GetSalesQuotationTypeQueryResponse>> GetSalesQuotationType(string companyId, CancellationToken cancellationToken);
        Task<Result<UpdatePurchaseQuotationTypeResponse>> UpdatePurchaseQuotationType(string companyId, PurchaseSalesQuotationNumberType purchaseQuotationNumberType, CancellationToken cancellationToken);
        Task<Result<UpdateSalesQuotationTypeResponse>> UpdateSalesQuotationType(string companyId, PurchaseSalesQuotationNumberType salesQuotationNumberType, CancellationToken cancellationToken);
    }
}
