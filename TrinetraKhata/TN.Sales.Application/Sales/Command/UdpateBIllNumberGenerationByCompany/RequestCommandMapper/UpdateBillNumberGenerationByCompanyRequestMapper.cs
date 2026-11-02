using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationByCompany.RequestCommandMapper
{
   public static class UpdateBillNumberGenerationByCompanyRequestMapper
    {
        public static UpdateBillNumberGenerationByCompanyCommand ToCommand(this UpdateBillNumberGenerationByCompanyRequest request, string companyId)
        {
            return new UpdateBillNumberGenerationByCompanyCommand
            (
               
               request.BillNumberGenerationType, 
                companyId
            );
        }
    }
}
