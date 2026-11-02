using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Authentication.Domain.Entities
{
    public class UserCompany : Entity
    {
        public UserCompany(
            ) : base(null)
        {

        }

        public UserCompany(
            string id,
            string userId,
            string companyId
            ) : base(id)
        {
            CompanyId = companyId;
            UserId = userId;

        }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
