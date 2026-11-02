using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Queries.GetMunicipalityByDistrictId;
using TN.Setup.Application.Setup.Queries.GetVdcByDistrictId;
using TN.Setup.Application.Setup.Queries.Vdc;
using TN.Setup.Application.Setup.Queries.VdcById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.ServiceInterface
{
    public interface IVdcServices
    {

        Task<Result<PagedResult<GetAllVdcResponse>>> GetAllVdc(PaginationRequest paginationRequest, CancellationToken cancellationToken);

        Task<Result<GetVdcByIdResponse>> GetVdcById(int vdcId, CancellationToken cancellationToken = default);

        Task<Result<List<GetVdcByDistrictIdResponse>>> GetVdcByDistrictId(int districtId, CancellationToken cancellationToken = default);






    }
}
