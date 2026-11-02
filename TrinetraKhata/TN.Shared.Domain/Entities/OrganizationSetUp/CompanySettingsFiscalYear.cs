using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.OrganizationSetUp
{
    public class CompanySettingsFiscalYear : Entity
    {
        public CompanySettingsFiscalYear(

            ) : base(null)
        {

        }

        public CompanySettingsFiscalYear(
            string id,
            string companySettingId,
            string fiscalYearId,
            bool isClosed,
            string userId,
            DateTime closedAt,
            bool isUpToCurrentFiscalYear,
            string fyName,
            string companyId,
            bool isFiscalYearStarted
            ) : base(id)
        {
            CompanySettingId = companySettingId;
            FiscalYearId = fiscalYearId;
            IsClosed = isClosed;
            UserId = userId;
            ClosedAt = closedAt;
            IsUpToCurrentFiscalYear = isUpToCurrentFiscalYear;
            FyName = fyName;
            CompanyId = companyId;
            IsFiscalYearStarted = isFiscalYearStarted;

        }

        public string CompanySettingId { get; set; }
        public CompanySettings CompanySetting { get; set; }

        public string FiscalYearId { get; set; }
        public FiscalYears FiscalYear { get; set; }

        public bool IsClosed { get; set; }
        public string UserId { get; set; }
        public DateTime ClosedAt { get; set; }
        public bool IsUpToCurrentFiscalYear { get; set; }
        public bool IsFiscalYearStarted { get; set; }

        public string FyName { get; set; }
        public string CompanyId { get; set; }
    }
}
        
