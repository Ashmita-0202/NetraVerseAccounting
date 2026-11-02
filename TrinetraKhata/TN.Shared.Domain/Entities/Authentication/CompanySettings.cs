using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Primitive;

namespace TN.Authentication.Domain.Entities
{
    public class CompanySettings: Entity
    {
        public CompanySettings(): base(null)
        {
            
        }

        public CompanySettings(
            string id,
            bool showTaxInSales,
            bool showTaxInPurchase,
            PurchaseReferencesType purchaseReferences,
            bool showReferenceNumberForSales,
            bool showExpiredDateInItem,
            bool showSerialNumberInItem,
            bool showSerialNumberForPurchase,
            bool showSerialNumberForSales,
            JournalReferencesType journalReferences,
            InventoryMethodType inventoryMethod,
            string companyId,
            string? currentFiscalYearId,
            bool? allowBackDatedEntry,
            TransactionNumberType receiptTransactionNumberType,
            TransactionNumberType paymentTransactionNumberType,
            TransactionNumberType incomeTransactionNumberType,
            TransactionNumberType expensesTransactionNumberType,


            bool showReturnNumberForPurchase,
            bool showReturnNumberForSales,
            bool showQuotationNumberForPurchase,
            bool showQuotationNumberForSales,
            PurchaseSalesReturnNumberType purchaseReturnNumberType,
            PurchaseSalesReturnNumberType salesReturnNumberType,
            PurchaseSalesQuotationNumberType purchaseQuotationNumberType,
            PurchaseSalesQuotationNumberType salesQuotationNumberType,
            string? userId
           


            ) : base(id)
        {
            ShowTaxInSales = showTaxInSales;
            ShowTaxInPurchase = showTaxInPurchase;
            PurchaseReferences = purchaseReferences;
            ShowReferenceNumberForSales = showReferenceNumberForSales;
            ShowExpiredDateInItem = showExpiredDateInItem;
            ShowSerialNumberInItem = showSerialNumberInItem;
            ShowSerialNumberForPurchase = showSerialNumberForPurchase;
            ShowSerialNumberForSales = showSerialNumberForSales;
            JournalReference = journalReferences;
            InventoryMethod = inventoryMethod;
            CompanyId = companyId;
            CurrentFiscalYearId = currentFiscalYearId;
            AllowBackDatedEntry = allowBackDatedEntry;
            ReceiptTransactionNumberType = receiptTransactionNumberType;
            PaymentTransactionNumberType = paymentTransactionNumberType;
            IncomeTransactionNumberType = incomeTransactionNumberType;
            ExpensesTransactionNumberType = expensesTransactionNumberType;
            ShowReturnNumberForPurchase = showReturnNumberForPurchase;
            ShowReturnNumberForSales = showReturnNumberForSales;
            ShowQuotationNumberForPurchase = showQuotationNumberForPurchase;
            ShowQuotationNumberForSales = showQuotationNumberForSales;
            PurchaseReturnNumberType = purchaseReturnNumberType;
            SalesReturnNumberType = salesReturnNumberType;
            PurchaseQuotationNumberType = purchaseQuotationNumberType;
            SalesQuotationNumberType = salesQuotationNumberType;
            UserId = userId;
            CompanySettingFiscalYears = new List<CompanySettingsFiscalYear>();


        }

        public bool ShowTaxInSales { get; set; }
        public bool ShowTaxInPurchase { get; set; }
        public PurchaseReferencesType PurchaseReferences { get; set; } = PurchaseReferencesType.Automatic;

        public bool ShowReferenceNumberForSales { get; set; }
        public bool ShowExpiredDateInItem { get; set; }
        public bool ShowSerialNumberInItem { get;set; }

        public bool ShowSerialNumberForPurchase { get; set; }
        public bool ShowSerialNumberForSales { get; set; }

        public JournalReferencesType JournalReference { get; set; } = JournalReferencesType.Automatic;
        public InventoryMethodType InventoryMethod { get; set; } = InventoryMethodType.FIFO;

        public TransactionNumberType ReceiptTransactionNumberType { get; set; } = TransactionNumberType.Automatic;
        public TransactionNumberType PaymentTransactionNumberType { get; set; } = TransactionNumberType.Automatic;
        public TransactionNumberType IncomeTransactionNumberType { get; set; } = TransactionNumberType.Automatic;
        public TransactionNumberType ExpensesTransactionNumberType { get; set; } = TransactionNumberType.Automatic;
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public string? CurrentFiscalYearId { get; set; }

        public FiscalYears CurrentFiscalYear { get; set; } = default!;
        public bool? AllowBackDatedEntry { get; set; } = false;


        public bool ShowReturnNumberForPurchase { get; set; }
        public bool ShowReturnNumberForSales { get; set; }
        public bool ShowQuotationNumberForPurchase { get; set; }
        public bool ShowQuotationNumberForSales { get; set; }
        public PurchaseSalesReturnNumberType PurchaseReturnNumberType { get; set; } = PurchaseSalesReturnNumberType.Automatic;
        public PurchaseSalesReturnNumberType SalesReturnNumberType { get; set; } = PurchaseSalesReturnNumberType.Automatic;
        public PurchaseSalesQuotationNumberType PurchaseQuotationNumberType { get; set; } = PurchaseSalesQuotationNumberType.Automatic;
        public PurchaseSalesQuotationNumberType SalesQuotationNumberType { get; set; } = PurchaseSalesQuotationNumberType.Automatic;

        public ICollection<CompanySettingsFiscalYear> CompanySettingFiscalYears { get; set; }

        public string? UserId{get; set; }
        public enum JournalReferencesType
        {
            Manual,
            Automatic
        }

        public enum PurchaseReferencesType
        {
            Manual,
            Automatic
        }

        public enum InventoryMethodType
        {
            FIFO=1,
            LIFO = 2,
            AverageWeighted=3
        }
        public enum ReferenceType
        {
            Purchase,
            Sales
        }

        public enum TransactionNumberType
        {
            Manual,
            Automatic
        }

        public enum PurchaseSalesReturnNumberType
        {
            Manual,
            Automatic
        }


        public enum  PurchaseSalesQuotationNumberType
        {
            Manual,
            Automatic

        }


    }
}
