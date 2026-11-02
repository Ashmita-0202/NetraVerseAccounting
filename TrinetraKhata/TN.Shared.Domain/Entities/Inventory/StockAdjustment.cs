using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Inventory
{
    public class StockAdjustment : Entity
    {
        public StockAdjustment(
            ): base(null)
        {
            
        }

        public StockAdjustment(
            string id,
            string itemId,
            double quantityChanged,
            ReasonType reason,
            DateTime adjustedAt,
            string adjustedBy,
            string companyId
            ) : base(id)
        {
            ItemId = itemId;
            QuantityChanged = quantityChanged;
            Reason = reason;
            AdjustedAt = adjustedAt;
            AdjustedBy = adjustedBy;
            CompanyId = companyId;

        }

        public string ItemId { get; private set; }
        public virtual Items Items { get; set; }
        public double QuantityChanged { get; private set; }
        public ReasonType Reason { get; private set; }
        public DateTime AdjustedAt { get; private set; }
        public string AdjustedBy { get; private set; }
        public string CompanyId { get; private set; }

        public enum ReasonType
        {
            DamagedLossTheft = 0,
            CountingError,
            SampleIssued
        }
    }
}
