using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdateInventoryMethodByCompany.RequestCommandMapper
{
    public static class UpdateInventoryMethodRequestMapper
    {
        public static UpdateInventoryMethodCommand  ToCommand(this UpdateInventoryMethodRequest request, string companyId)
        {
            return new UpdateInventoryMethodCommand
                (
                    
                    request.inventoryMethod,
                    companyId
                );
        }
    }
}
