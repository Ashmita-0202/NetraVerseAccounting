using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.ImportExcelForItems
{
    public record ItemsExcelRequest
    (
        IFormFile formFile
        );
    
}
