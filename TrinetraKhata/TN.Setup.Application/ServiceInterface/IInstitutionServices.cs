using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Command.AddInstitution;
using TN.Setup.Application.Setup.Command.AddOrganization;
using TN.Setup.Application.Setup.Command.UpdateInstitution;
using TN.Setup.Application.Setup.Command.UpdateOrganization;
using TN.Setup.Application.Setup.Queries.District;
using TN.Setup.Application.Setup.Queries.DistrictById;
using TN.Setup.Application.Setup.Queries.GetDistrictByProvinceId;
using TN.Setup.Application.Setup.Queries.GetOrganizationByProvinceId;
using TN.Setup.Application.Setup.Queries.Institution;
using TN.Setup.Application.Setup.Queries.InstitutionById;
using TN.Setup.Application.Setup.Queries.InstitutionByOrganizationId;
using TN.Setup.Application.Setup.Queries.Organization;
using TN.Setup.Application.Setup.Queries.OrganizationById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.ServiceInterface
{
    public interface IInstitutionServices
    {

        Task<Result<AddInstitutionResponse>> Add(AddInstitutionCommand addInstitutionCommand);
        Task<Result<UpdateInstitutionResponse>> Update(string institutionId, UpdateInstitutionCommand updateInstitutionCommand);
        Task<Result<PagedResult<GetAllInstitutionResponse>>> GetAllInstitution(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<GetInstitutionByIdResponse>> GetInstitutionById(string institutionId, CancellationToken cancellationToken = default);
        Task<Result<List<GetInstitutionByOrganizationIdResponse>>> GetInstitutionByOrganizationId(string organizationId, CancellationToken cancellationToken = default);
        Task<Result<bool>> Delete(string Id, CancellationToken cancellationToken);

    }
}
