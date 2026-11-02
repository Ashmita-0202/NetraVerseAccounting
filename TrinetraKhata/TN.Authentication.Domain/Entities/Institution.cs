using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Authentication.Domain.Entities
{
    public class Institution : Entity
    {
        public Institution() : base(null)
        {

        }
        public Institution(
            string id,
            string name,
            string address,
            string email,
            string shortName,
            string contactNumber,
            string contactPerson,
            string pan,
            string imageUrl,
            DateTime createdDate,
            string createdBy,
            DateTime modifiedDate,
            string modifiedBy,
            bool isDeleted,
            string organizationId

            ) : base(id)
        {
            Name = name;
            Address = address;
            Email = email;
            ShortName = shortName;
            ContactNumber = contactNumber;
            ContactPerson = contactPerson;
            PAN = pan;
            ImageUrl = imageUrl;
            CreatedDate = createdDate;
            CreatedBy = createdBy;
            ModifiedDate = modifiedDate;
            ModifiedBy = modifiedBy;
            IsDeleted = isDeleted;
            OrganizationId = organizationId;
            Companies = new List<Company>();
            Users = new List<ApplicationUser>();


        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ShortName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactPerson { get; set; }
        public string PAN { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public string OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }

    }
}
