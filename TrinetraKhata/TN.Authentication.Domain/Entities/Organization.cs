using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Authentication.Domain.Entities
{
    public class Organization: Entity
    {
        public Organization() : base(null)
        {

        }

        public Organization(
            string id,
            string name,
            string address,
            string email,
            string phoneNumber,
            string mobileNumber,
            string logo,
            int provinceId

            ) : base(id)
        {
            Name = name;
            Address = address;
            Email = email;
            PhoneNumber = phoneNumber;
            MobileNumber = mobileNumber;
            Logo = logo;
            ProvinceId = provinceId;
            Institutions = new List<Institution>();


        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? MobileNumber { get; set; }
        public string? Logo { get; set; }

        public int? ProvinceId { get; set; }
        public Province Provinces { get; set; }

        public ICollection<Institution> Institutions { get; set; }
    }
}
