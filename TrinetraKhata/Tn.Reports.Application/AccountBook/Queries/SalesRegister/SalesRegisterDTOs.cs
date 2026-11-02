using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.AccountBook.Queries.SalesRegister
{
    public record SalesRegisterDTOs
    (
        string? companyId,
        string? startDate,
        string? endDate
        );
}
