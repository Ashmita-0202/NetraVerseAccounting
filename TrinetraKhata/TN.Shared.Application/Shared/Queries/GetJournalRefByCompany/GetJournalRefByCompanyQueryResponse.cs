using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Authentication.Domain.Entities.CompanySettings;

namespace TN.Shared.Application.Shared.Queries.GetJournalRefByCompany
{
    public record  GetJournalRefByCompanyQueryResponse
    (
        JournalReferencesType journalReferences= default,
            string companyId="",
            string? journalRefNo=""

    );
}
