using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddItems.RequestCommandMapper
{
  public static class AddItemRequestMapper
    {
        public static AddItemCommand ToCommand(this AddItemRequest request) 
        {
            return new AddItemCommand
                (
                    request.name,
                    request.price,
                    request.itemGroupId,
                    request.unitId,
                    request.sellingPrice,
                    request.costPrice,
                    request.barCodeField,
                    request.openingStockQuantity,
                    request.hsCode,
                    request.minimumLevel,
                    request.hasSerial,
                    request.conversionFactorId,
                   
                    request.isItems,
                    request.isVatEnables,
                    request.isConversionFactor,
                    request.stockCenterId,
                    request.hasExpiryAndManufacture,
                    request.hasBatchNumber,
                    request.manufactureAndExpiries,
                    request.batchNumbers,
                     request.serialNumbers

                );
        }
    }
}
