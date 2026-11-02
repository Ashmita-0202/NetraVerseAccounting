using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TN.Account.Application.Account.Command.ImportExcelForLedgers
{
    public record  LedgerExcelRequest
    (

         IFormFile formFile

    );
}
