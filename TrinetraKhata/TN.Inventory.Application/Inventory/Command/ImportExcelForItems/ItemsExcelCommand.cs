using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.ImportExcelForItems
{
    public record ItemsExcelCommand
    (
        IFormFile formFile
        ) : IRequest<Result<ItemsExcelResponse>>;
    
}
