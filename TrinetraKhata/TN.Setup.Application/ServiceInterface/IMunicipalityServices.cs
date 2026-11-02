using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Queries.GetMunicipalityByDistrictId;
using TN.Setup.Application.Setup.Queries.GetVdcByDistrictId;
using TN.Setup.Application.Setup.Queries.Municipality;
using TN.Setup.Application.Setup.Queries.MunicipalityById;
using TN.Setup.Application.Setup.Queries.Province;
using TN.Setup.Application.Setup.Queries.ProvinceById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.ServiceInterface
{
    public interface IMunicipalityServices
    {
        Task<Result<GetMunicipalityByIdResponse>> GetMunicipalityById(int municipalityId, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<GetAllMunicipalityResponse>>> GetAllMunicipality(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<List<GetMunicipalityByDistrictIdResponse>>> GetMunicipalityByDistrictId(int districtId, CancellationToken cancellationToken = default);


    }
}

