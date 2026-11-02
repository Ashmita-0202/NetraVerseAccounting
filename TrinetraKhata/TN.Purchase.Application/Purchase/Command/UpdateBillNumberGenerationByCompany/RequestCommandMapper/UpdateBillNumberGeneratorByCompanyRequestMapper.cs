using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationByCompany.RequestCommandMapper
{
    public static class UpdateBillNumberGeneratorByCompanyRequestMapper
    {
        public static UpdateBillNumberGeneratorByCompanyCommand ToCommand(this UpdateBillNumberGeneratorByCompanyRequest request, string companyId)
        {
            return new UpdateBillNumberGeneratorByCompanyCommand
                (
                request.BillNumberGenerationType,
                companyId
                );
            
        }
    }
}
