using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;

namespace TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberByCompany.RequestCommandMapper
{
    public static class UpdateSalesReferenceRequestMapper
    {
        public static UpdateSalesReferenceNumberCommand ToCommand(this  UpdateSalesReferenceNumberRequest request,string companyId) 
        {
            return new UpdateSalesReferenceNumberCommand
                (
                    
                    request.showReferenceNumberForSales,
                    companyId

                );
        
        }

    }
}
