using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.Modules
{
    public record  GetAllModulesResponse
    (
            string Id="",
            string Name="",
            string? Rank="",
            string? TargetUrl = "",
            bool IsActive=true

    );
}
