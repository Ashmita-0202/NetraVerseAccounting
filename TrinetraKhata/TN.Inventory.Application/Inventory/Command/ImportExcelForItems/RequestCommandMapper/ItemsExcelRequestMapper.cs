using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.ImportExcelForItems.RequestCommandMapper
{
    public static class ItemsExcelRequestMapper
    {
        public static ItemsExcelCommand ToCommand(this ItemsExcelRequest request)
        {
            return new ItemsExcelCommand
                (
                request.formFile
                );
        }
    }
}
