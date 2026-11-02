using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Queries.Province;
using TN.Setup.Application.Setup.Queries.ProvinceById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.ServiceInterface
{
    public interface IProvinceServices
    {
        Task<Result<GetProvinceByIdResponse>> GetProvinceById(int provinceId, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<GetAllProvinceResponse>>> GetAllProvince(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
    }
}
