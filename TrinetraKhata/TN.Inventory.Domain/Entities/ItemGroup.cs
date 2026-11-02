using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Inventory.Domain.Entities
{
    public class ItemGroup : Entity
    {
        public ItemGroup() : base(null)
        {

        }

        public ItemGroup(
            string id,
            string name,
            string description,
            bool isPrimary,
            string? itemsGroupsId,
            string companyId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            Name = name;
            Description = description;
            IsPrimary = isPrimary;
            ItemsGroupId = itemsGroupsId;
            CompanyId = companyId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;

            Items = new List<Items>();
            SubGroups = new List<ItemGroup>();

        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrimary { get; set; }
       
        // Self-referencing foreign key
        public string? ItemsGroupId { get; set; }
        public ItemGroup? ParentGroup { get; set; }  // Reference to the parent group

        // Collection for child groups
        public ICollection<ItemGroup> SubGroups { get; set; }
        public ICollection<Items> Items { get; set; }
        public string CompanyId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

    }
}