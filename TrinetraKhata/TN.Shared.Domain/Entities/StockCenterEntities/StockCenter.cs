using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Sales.Domain.Entities;
using TN.Shared.Domain.Entities.Purchase;
using TN.Shared.Domain.Entities.Sales;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.StockCenterEntities
{
    public class StockCenter : Entity
    {
        public StockCenter() : base(null)
        {

        }

        public StockCenter(
            string id,
            string name,
            string? address,
            string? companyId,
            string? createdBy,
            DateTime? createdAt
            ) : base(id)
        {
            Name = name;
            Address = address;
            CompanyId = companyId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            PurchaseDetails = new List<PurchaseDetails>();
            PurchaseQuotationDetails = new List<PurchaseQuotationDetails>();
            SalesDetails = new List<SalesDetails>();
            SalesQuotationDetails = new List<SalesQuotationDetails>();
            Items = new List<Items>();
            Inventories = new List<Inventories>();
            PurchaseReturnDetails = new List<PurchaseReturnDetails>();
            SalesReturnDetails = new List<SalesReturnDetails>();
        }

        public string Name { get; set; }
        public string? Address { get; set; }

        public string? CompanyId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public ICollection<PurchaseDetails> PurchaseDetails { get; set; }

        public ICollection<PurchaseQuotationDetails> PurchaseQuotationDetails { get; set; }

        public ICollection<SalesDetails> SalesDetails { get; set; }
        public ICollection<SalesQuotationDetails> SalesQuotationDetails { get; set; }
        public ICollection<Items> Items { get; set; }
        public ICollection<Inventories> Inventories { get; set; }

        public ICollection<PurchaseReturnDetails> PurchaseReturnDetails { get; set; }

        public ICollection<SalesReturnDetails> SalesReturnDetails { get; set; }
    }
        
}
