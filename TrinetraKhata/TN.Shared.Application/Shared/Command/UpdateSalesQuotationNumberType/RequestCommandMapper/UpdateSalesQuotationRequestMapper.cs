using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdateSalesQuotationNumberType.RequestCommandMapper
{
    public static class UpdateSalesQuotationRequestMapper
    {
       public static UpdateSalesQuotationTypeCommand ToCommand(this UpdateSalesQuotationTypeRequest request,string companyId)
        {
            return new UpdateSalesQuotationTypeCommand(
                companyId,
                request.salesQuotationNumberType
            );
        }
    }
}
