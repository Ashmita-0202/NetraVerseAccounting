using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Queries.InventoriesLogsById
{
    public record  GetInventoriesLogsByIdQuery
    (string id):IRequest<Result<GetInventoriesLogsByIdQueryResponse>>;
    
}
