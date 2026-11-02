using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.UpdateCustomerCategory.RequestCommandMapper
{
    public static class UpdateCustomerCategoryRequestMapper
    {
        public static UpdateCustomerCategoryCommand ToCommand(this UpdateCustomerCategoryRequest request, string id)
        { 
            return new UpdateCustomerCategoryCommand
                (
                    id,
                    request.name,
                    request.createdAt,
                    request.isEnabled,
                    request.customerId
                
                );
        }
    }
}
