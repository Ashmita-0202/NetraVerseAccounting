using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using TN.Shared.Domain.Abstractions;

namespace TN.Transactions.Application.Transactions.Command.ImportExcelForReceipt
{
    public record ReceiptExceCommand
    (
        IFormFile formFile
    ):IRequest<Result<ReceiptExcelResponse>>;
}
   
