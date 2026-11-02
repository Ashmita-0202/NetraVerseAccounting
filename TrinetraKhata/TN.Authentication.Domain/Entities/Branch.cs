using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Authentication.Domain.Entities
{
    public class Branch: Entity
    {
        public Branch() : base(null)
        {

        }

        public Branch(
            string id,
             string name,
            string address,
            string shortName,
            string email,
            string contactNumber,
            string contactPerson,
            bool isEnabled,
            DateTime createdDate,
            string createdBy,
            DateTime modifiedDate,
            string modifiedBy,
            bool isDeleted,
            string companyId
            ) : base(id)
        {
            Name = name;
            Address = address;
            ShortName = shortName;
            Email = email;
            ContactNumber = contactNumber;
            ContactPerson = contactPerson;
            IsEnabled = isEnabled;
            CreatedDate = createdDate;
            CreatedBy = createdBy;
            ModifiedDate = modifiedDate;
            ModifiedBy = modifiedBy;
            IsDeleted = isDeleted;
            CompanyId = companyId;


        }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ShortName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string ContactPerson { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public string CompanyId { get; set; }
        public Company Companies { get; set; }
    }
}
