using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.OrganizationSetUp
{
    public class FiscalYears : Entity
    {
        public FiscalYears(
            ): base(null)
        {
            
        }

        public FiscalYears(
            string id,
            string fyName,
            DateTime startDate,
            DateTime endDate
            
            ) : base(id)
        {
            FyName = fyName;
            StartDate = startDate;
            EndDate = endDate;
            CompanySettingFiscalYears = new List<CompanySettingsFiscalYear>();
       
        }

        public string FyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<CompanySettingsFiscalYear> CompanySettingFiscalYears { get; set; }
    }
}
