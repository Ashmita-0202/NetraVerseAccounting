using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Account
{
    public class BillSundry : Entity
    {

        public BillSundry(
            string id,
             string name,
        BillSundryType billType,
        decimal? defaultValue,

        BillSundryNature billSundryNature,
        bool isCOGSAffected,
        bool isCOGPAffected,
        bool isCOGSTAffected,
        bool isSalesAccountingAffected,
        bool isPurchaseAccountingAffected,
        bool isSalesAmountAdjusted,
        bool isPurchaseAmountAdjusted,
        bool customerAmountAdjusted,
        bool vendorAmountAdjusted,
        string? salesAdjustedLedgerId,
        string? customerAdjustedLedgerId,
        string? purchaseAdjustedLedgerId,
        string? vendorAdjustedLedgerId,
        CalculationType calculationType,
        CalculationTypeOf calculationTypeOf,
        string? companyId,
        string? createdBy,
        DateTime? createdAt,
        bool isActive

            ) : base(id)
        {
            Name = name;
            BillType = billType;
            DefaultValue = defaultValue;
            BillSundryNature = billSundryNature;
            IsCOGSAffected = isCOGSAffected;
            IsCOGPAffected = isCOGPAffected;
            IsCOGSTAffected = isCOGSTAffected;
            IsSalesAccountingAffected = isSalesAccountingAffected;
            IsPurchaseAccountingAffected = isPurchaseAccountingAffected;
            IsSalesAmountAdjusted = isSalesAmountAdjusted;
            IsPurchaseAmountAdjusted = isPurchaseAmountAdjusted;
            CustomerAmountAdjusted = customerAmountAdjusted;
            VendorAmountAdjusted = vendorAmountAdjusted;
            CalculationType = calculationType;
            CalculationTypeOf = calculationTypeOf;
            SalesAdjustedLedgerId = salesAdjustedLedgerId;
            CustomerAdjustedLedgerId = customerAdjustedLedgerId;
            PurchaseAdjustedLedgerId = purchaseAdjustedLedgerId;
            VendorAdjustedLedgerId = vendorAdjustedLedgerId;
            CompanyId = companyId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            IsActive = isActive;
            BillSundryDetails = new List<BillSundryDetails>();
        }
        public string Name { get; set; }
        public BillSundryType BillType { get; set; }
        public decimal? DefaultValue { get; set; }
        public BillSundryNature BillSundryNature { get; set; }
        public bool IsCOGSAffected { get; set; }
        public bool IsCOGPAffected { get; set; }
        public bool IsCOGSTAffected { get; set; }
        public bool IsSalesAccountingAffected { get; set; }
        public bool IsPurchaseAccountingAffected { get; set; }
        public bool IsSalesAmountAdjusted { get; set; }
        public bool IsPurchaseAmountAdjusted { get; set; }
        public bool CustomerAmountAdjusted { get; set; }
        public bool VendorAmountAdjusted { get; set; }
        public CalculationType CalculationType { get; set; }
        public CalculationTypeOf CalculationTypeOf { get; set; }

        public string? SalesAdjustedLedgerId { get; set; }

        public string? CustomerAdjustedLedgerId { get; set; }
        public string? PurchaseAdjustedLedgerId { get; set; }
        public string? VendorAdjustedLedgerId { get; set; }

        public string? CompanyId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }

        public bool IsActive { get; set; }
        public ICollection<BillSundryDetails> BillSundryDetails { get; set; }

    }


    public enum BillSundryType
    {
        Additive,
        Subtractive
    }

    public enum BillSundryNature
    {
        [Description("Charge/Expense (Dr)")]
        ChargeOrExpenseDr,

        [Description("Discount/Income (Cr)")]
        DiscountOrIncomeCr,

        [Description("Tax/Input (Dr)")]
        TaxOrInputDr,

        [Description("Tax/Output (Cr)")]
        TaxOrOutputCr,

        [Description("VAT")]
        VAT
    }


    public enum CalculationTypeOf
    {

        [Description("Sub Total Amount")]
        SubTotalAmount,

        [Description("Taxable Amount")]
        TaxableAmount,

        [Description("Amount After VAT")]
        AmountAfterVAT,
    }

    public enum CalculationType
    {
        [Description("Absolute Amount")]
        AbsoluteAmount,
        [Description("Percentage")]
        Percentage
    }

   
}
