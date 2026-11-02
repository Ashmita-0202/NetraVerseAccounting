using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.AddCustomerCategory.RequestCommandMapper
{
 public static class AddCustomerCategoryRequestMapper
    {
        public static AddCustomerCategoryCommand ToCommand(this AddCustomerCategoryRequest request) 
        {
            return new AddCustomerCategoryCommand
                (
                    request.name,
                    request.createdAt,
                    request.isEnabled,
                    request.customerId
                
                );

        }
    }
}
