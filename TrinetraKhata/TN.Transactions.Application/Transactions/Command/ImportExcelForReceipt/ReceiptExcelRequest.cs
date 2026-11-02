using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TN.Transactions.Application.Transactions.Command.ImportExcelForReceipt
{
    public record  ReceiptExcelRequest
    (
          IFormFile formFile

    );
}
