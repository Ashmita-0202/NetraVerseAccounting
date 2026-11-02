using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.UpdateDate
{
    public record  UpdateDateResponse
    (
        string userId,
        DateTime Date
        
    );
}
