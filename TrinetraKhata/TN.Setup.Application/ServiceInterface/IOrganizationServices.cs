using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Command.AddOrganization;
using TN.Setup.Application.Setup.Command.UpdateModules;
using TN.Setup.Application.Setup.Command.UpdateOrganization;
using TN.Setup.Application.Setup.Queries.District;
using TN.Setup.Application.Setup.Queries.DistrictById;
using TN.Setup.Application.Setup.Queries.GetDistrictByProvinceId;
using TN.Setup.Application.Setup.Queries.GetOrganizationByProvinceId;
using TN.Setup.Application.Setup.Queries.Organization;
using TN.Setup.Application.Setup.Queries.OrganizationById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.ServiceInterface
{
    public interface IOrganizationServices
    {
        Task<Result<AddOrganizationResponse>> Add(AddOrganizationCommand addOrganizationCommand);
        Task<Result<PagedResult<GetAllOrganizationResponse>>> GetAllOrganization(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<GetOrganizationByIdQueryResponse>> GetOrganizationById(string organizationId, CancellationToken cancellationToken = default);
        Task<Result<List<GetOrganizationByProvinceIdResponse>>> GetOrganizationByProvinceId(int provinceId, CancellationToken cancellationToken = default);
        Task<Result<UpdateOrganizationResponse>> Update(string organizationId, UpdateOrganizationCommand updateOrganizationCommand);
        Task<Result<bool>> Delete(string Id, CancellationToken cancellationToken);

    }
}
