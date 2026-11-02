using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.DeleteItem
{
    public record DeleteItemCommand
   (string id):IRequest<Result<bool>>;
}
