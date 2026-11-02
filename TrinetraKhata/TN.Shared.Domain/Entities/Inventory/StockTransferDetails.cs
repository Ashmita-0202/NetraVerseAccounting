using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Inventory
{
    public class StockTransferDetails : Entity
    {
        public StockTransferDetails(): base(null)
        {
            
        }

        public StockTransferDetails
            (
            string id,
            string transferDate,
            string stockCenterNumber,
            string fromStockCenterId,
            string toStockCenterId,
            string narration,
            string createdBy,
            DateTime createdAt,
            string updatedBy,
            DateTime updatedAt,
            string companyId,
            List<StockTransferItems> stockTransferItems


            ) : base(id)
        {
            TransferDate = transferDate;
            StockCenterNumber = stockCenterNumber;
            FromStockCenterId = fromStockCenterId;
            ToStockCenterId = toStockCenterId;
            Narration = narration;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            CompanyId = companyId;
            StockTransferItems = stockTransferItems;


        }

        public string TransferDate { get; set; }
        public string StockCenterNumber { get; set; }
        public string FromStockCenterId { get;set; }
        public string ToStockCenterId { get; set; }
        public string Narration { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CompanyId { get; set; }
        public ICollection<StockTransferItems> StockTransferItems { get; set; }
    }
}
