using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.OpeningStockByCompanyId;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.ServiceInterface
{
    public interface IOpeningStockService
    {
        Task<Result<GetOpeningStockQueryResponse>> GetOpeningStock(string companyId, CancellationToken cancellationToken = default);
    }
}
