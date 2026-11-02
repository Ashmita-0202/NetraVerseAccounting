using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.UpdateItemGroup.RequestCommandMapper
{
   public static class UpdateItemGroupRequestMapper
   {
       public static UpdateItemGroupCommand ToCommand(this UpdateItemGroupRequest request, string id) 
        {
            return new UpdateItemGroupCommand
                   (
                        request.id,
                        request.name,
                        request.description,
                        request.isPrimary,
                        request.itemsGroupId

                   );
        }
    }
}
