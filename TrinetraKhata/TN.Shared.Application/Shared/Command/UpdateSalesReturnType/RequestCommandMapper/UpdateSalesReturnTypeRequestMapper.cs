using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Authentication.Domain.Entities.CompanySettings;

namespace TN.Shared.Application.Shared.Command.UpdateSalesReturnType.RequestCommandMapper
{
    public static class UpdateSalesReturnTypeRequestMapper
    {
        public static UpdateSalesReturnTypeCommand ToCommand(this UpdateSalesReturnTypeRequest request,string companyId)
        {
            return new UpdateSalesReturnTypeCommand
                (
                     companyId,
                     request.salesReturnNumberType


                );
        }
    }
}
