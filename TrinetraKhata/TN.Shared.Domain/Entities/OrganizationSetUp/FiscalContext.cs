using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.Entities.OrganizationSetUp
{
    public class FiscalContext
    {
        public string? CurrentFiscalYearId { get; set; }
        public string? CurrentCompanyId { get; set; }
    }
}
