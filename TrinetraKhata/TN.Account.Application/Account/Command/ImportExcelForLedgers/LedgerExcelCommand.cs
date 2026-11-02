using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.ImportExcelForLedgers
{
    public record  LedgerExcelCommand
   (IFormFile formFile): IRequest<Result<LedgerExcelResponse>>;
}
