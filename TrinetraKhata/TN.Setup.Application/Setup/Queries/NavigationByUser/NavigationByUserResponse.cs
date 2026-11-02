using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.NavigationByUser
{
    public record NavigationByUserResponse
    (
      string id,
      string name,
      string? role,
      string? targetUrl,
      string? rank,
      bool isActive,
      List<SubModulesResponse> subModulesResponse
        );
}
