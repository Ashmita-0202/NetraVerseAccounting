using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdateJournalRefByCompany.RequestCommandMapper
{
    public static class UpdateJournalRefByCompanyRequestMapper
    {
        public static UpdateJournalRefByCompanyCommand ToCommand(this UpdateJournalRefByCompanyRequest request, string companyId) 
        {

            return new UpdateJournalRefByCompanyCommand
                (
                   
                    request.journalReferences,
                     companyId
                );
        }
    }
}
