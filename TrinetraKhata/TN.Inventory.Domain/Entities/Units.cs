using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Inventory.Domain.Entities
{
    public class Units : Entity
    {
        public Units() : base(null)
        {

        }

        public Units(
            string id,
            string name,
            DateTime createdAt,
            string userId,
            DateTime updatedAt,
            string updatedBy,
            bool isEnabled,
            string companyId
            ) : base(id)
        {
            Name = name;
            CreatedAt = createdAt;
            UserId = userId;
            UpdatedAt = updatedAt;
            UpdatedBy = updatedBy;
            IsEnabled = isEnabled;
            FromConversions = new List<ConversionFactor>();
            ToConversions = new List<ConversionFactor>();
            Items = new List<Items>();
            CompanyId = companyId;
        }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public DateTime UpdatedAt { get; set; }
        = DateTime.Now;
        public string UpdatedBy { get; set; }
        public bool IsEnabled { get; set; }

        //Navigation Property
        public ICollection<ConversionFactor> FromConversions { get; set; }
        public ICollection<ConversionFactor> ToConversions { get; set; }
        public ICollection<Items> Items { get; set; }
        public string CompanyId { get; set; }
    }
}
