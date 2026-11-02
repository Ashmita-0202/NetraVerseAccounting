using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.Shared.Command.CloseFiscalYear;
using TN.Shared.Application.Shared.Queries.FiscalYearStartDate;
using TN.Shared.Application.Shared.Queries.GetAllCurrentFiscalYear;
using TN.Shared.Application.Shared.Queries.GetSelectedFiscalYear;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Shared.Application.ServiceInterface
{
    public interface IFiscalYearService
    {

        Task<FiscalYears?> GetCurrentFiscalYearFromSettingsAsync();
        Task<(string Id, string FyName)> GetFiscalYearIdForDateAsync(DateTime date);
        bool IsDateInFiscalYear(DateTime date, FiscalYears fiscalYear);

        Task<Result<FiscalYearStartDateResponse>> GetFiscalYearStartDate();

        Task<Result<CloseFiscalYearResponse>> CloseFiscalYear(CloseFiscalYearCommand request);

        Task<Result<List<GetSelectedFiscalYearQueryResponse>>> GetSelectedFiscalYear(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);

    }
}
