using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Queries.District;
using TN.Setup.Application.Setup.Queries.DistrictById;
using TN.Setup.Application.Setup.Queries.GetDistrictByProvinceId;
using TN.Setup.Application.Setup.Queries.Province;
using TN.Setup.Application.Setup.Queries.ProvinceById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.ServiceInterface
{
    public interface IDistrictServices
    {

        Task<Result<GetDistrictByIdResponse>> GetDistrictById(int districtId, CancellationToken cancellationToken = default);

        Task<Result<List<GetDistrictByProvinceIdResponse>>> GetDistrictByProvinceId(int provinceId, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<GetAllDistrictResponse>>> GetAllDistrict(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
    }
}
