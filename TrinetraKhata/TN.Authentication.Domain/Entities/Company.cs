using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Authentication.Domain.Entities
{
    public class Company: Entity
    {
        public Company() : base(null)
        {


        }

        public Company(
            string id,
            string name,
            string address,
            string shortName,
            string email,
            string contactNumber,
            string contactPerson,
            string pan,
            string imageUrl,
            bool isEnabled,
            string institutionId,
            DateTime createdDate,
            string createdBy,
            DateTime modifiedDate,
            string modifiedBy,
            bool isDeleted,
            BillNumberGenerationType billNumberGenerationTypeForPurchase,
            BillNumberGenerationType billNumberGenerationTypeForSales
            ) : base(id)
        {
            Name = name;
            Address = address;
            ShortName = shortName;
            Email = email;
            ContactNumber = contactNumber;
            ContactPerson = contactPerson;
            PAN = pan;
            ImageUrl = imageUrl;
            IsEnabled = isEnabled;
            InstitutionId = institutionId;
            CreatedDate = createdDate;
            CreatedBy = createdBy;
            ModifiedDate = modifiedDate;
            ModifiedBy = modifiedBy;
            IsDeleted = isDeleted;
            Branches = new List<Branch>();
            UserCompany = new List<UserCompany>();
            CompanySettings = new List<CompanySettings>();
            BillNumberGenerationTypeForPurchase = billNumberGenerationTypeForPurchase;
            BillNumberGenerationTypeForSales = billNumberGenerationTypeForSales;


        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string ShortName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string ContactPerson { get; set; }
        public string PAN { get; set; }
        public string ImageUrl { get; set; }
        public bool IsEnabled { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public string InstitutionId { get; set; }
        public Institution Institutions { get; set; }
        public BillNumberGenerationType BillNumberGenerationTypeForPurchase { get; set; } = BillNumberGenerationType.Manual;
        public BillNumberGenerationType BillNumberGenerationTypeForSales{ get; set; } = BillNumberGenerationType.Manual;

        public virtual ICollection<Branch> Branches { get; set; }
        public virtual ICollection<UserCompany> UserCompany { get; set; }

        public virtual ICollection<CompanySettings> CompanySettings { get; set; }

        public enum BillNumberGenerationType
        {
            Manual,   
            Automatic 
        }
    }
}
