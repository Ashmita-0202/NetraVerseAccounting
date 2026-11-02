
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;

namespace TN.Authentication.Domain.Entities
{

    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        //public string? DepartmentId { get; set; }
        //public string? ProfilePictureName { get; set; }
        //public string? ProfilePictureUrl { get; set; }
        //public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        //public ICollection<UserRole> UserRoles { get; set; }
        public string? InstitutionId { get; set; }


        //Navigation Property
        public virtual Institution Institution { get; set; }
        public virtual ICollection<JournalEntry> JournalEntries {  get; set; } = new List<JournalEntry>();
        public virtual ICollection<UserCompany> UserCompany { get; set; } = new List<UserCompany>();
    }
}
