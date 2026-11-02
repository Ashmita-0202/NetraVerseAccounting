using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Notification
{
    public class LoggedStatus : Entity
    {
        public LoggedStatus(): base(null)
        {

        }
        public LoggedStatus(
            string id,
            bool? isLoggedIn): base(id)
        {
            IsLoggedIn = isLoggedIn;
            
        }

        public bool? IsLoggedIn { get; set; }
    }
}
