using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Command.AddCompany;
using TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase;
using TN.Setup.Application.Setup.Command.UpdateCompany;
using TN.Setup.Application.Setup.Queries.Company;
using TN.Setup.Application.Setup.Queries.CompanyById;
using TN.Setup.Application.Setup.Queries.CompanyByInstitutionId;
using TN.Setup.Application.Setup.Queries.FilterCompanyByDate;
using TN.Setup.Application.Setup.Queries.GetCompanyDetailsByCompanyId;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using static TN.Authentication.Domain.Entities.Company;

namespace TN.Setup.Application.ServiceInterface
{
    public interface ICompanyServices
    {
        Task<Result<PagedResult<GetAllCompanyQueryResponse>>> GetAllCompany(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<AddCompanyResponse>> Add(AddCompanyCommand addCompanyCommand);
        Task<Result<UpdateCompanyResponse>> Update(string companyId, UpdateCompanyCommand updateCompanyCommand);
        Task<Result<GetCompanyByIdResponse>> GetCompanyById(string companyId, CancellationToken cancellationToken = default);
        Task<Result<List<GetCompanyByInstitutionIdResponse>>> GetCompanyByInstitutionId(string institutionId, CancellationToken cancellationToken = default);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<UpdateBillNumberStatusForPurchaseResponse>> Update(string id, BillNumberGenerationType type);
        Task<Result<IEnumerable<FilterCompanyByDateQueryResponse>>> GetCompanyFilter(FilterCompanyDTOs filterCompanyDTOs , CancellationToken cancellationToken);
        Task<Result<List<GetCompanyDetailsByCompanyIdQueryResponse>>> GetCompanyDetailsByInstitutionId(string institutionId, CancellationToken cancellationToken = default);
        

    
    }
}
