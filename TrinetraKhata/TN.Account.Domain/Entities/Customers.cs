using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Enum;
using TN.Shared.Domain.Primitive;

namespace TN.Account.Domain.Entities
{
    public class Customers : Entity
    {
        public Customers(
            string id,
            string fullName,
            string address,
            string contact,
            string? email,
            string? description,
            string? panNo,
            int? maxDueDates,
            decimal? maxCreditLimit,
            bool? isEnabled,
            decimal? openingBalance,
            BalanceType? balanceType,
            bool? isSmsEnabled,
            bool? isEmailEnabled,
            string ledgerId
            ) : base(id)
        {
            FullName = fullName;
            Address = address;
            Contact = contact;
            Email = email;
            Description = description;
            PanNo = panNo;
            MaxDueDates = maxDueDates;
            MaxCreditLimit = maxCreditLimit;
            IsEnabled = isEnabled;
            OpeningBalance = openingBalance;
            BalanceType = balanceType;
            IsSmsEnabled = isSmsEnabled;
            IsEmailEnabled = isEmailEnabled;
            LedgerId = ledgerId ;
            CustomerCategories = new List<CustomerCategory>() ;
        }

        public string FullName {  get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public string? PanNo { get; set; }
        public int? MaxDueDates { get; set; }
        public decimal? MaxCreditLimit { get; set; }
        public bool? IsEnabled { get; set; }
        public decimal? OpeningBalance { get; set; }
        public BalanceType? BalanceType { get; set; }
        public bool? IsSmsEnabled { get; set; }
        public bool? IsEmailEnabled { get;set; }

        public string LedgerId { get; set; }
        public Ledger Ledger { get; set; }

        public ICollection<CustomerCategory> CustomerCategories { get; set; }
    }
}
