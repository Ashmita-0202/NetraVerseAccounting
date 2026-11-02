using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.UpdateItem.RequestCommandMapper
{
   public static  class UpdateItemRequestMapper
    {
       public static UpdateItemCommand Tocommand(this UpdateItemRequest request, string id) 
        {
          return new UpdateItemCommand
                (
                    id,
                    request.name,
                    request.price,
                    request.itemGroupId,
                    request.unitId,
                    request.sellingPrice,
                    request.costPrice,
                    request.barCodeField,
                    request.expiredDate,
                    request.openingStockQuantity,
                    request.hasCode,
                    request.minimumLevel,
                    request.hsSerial,
                    request.conversionFactorId,
                    request.isItems,
                    request.isVatEnables,
                    request.isConversionFactor,
                    request.stockCenterId,
                    request.serialNumbers
                   
                );
       }
    }
}
