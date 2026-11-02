using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Setup.Application.Setup.Command.AddInstitution;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.Company;

namespace TN.Setup.Application.Setup.Command.AddCompany
{
    public record AddCompanyCommand
    (
            string name,
            string address,
            string shortName,
            string email,
            string contactNumber,
            string contactPerson,
            string pan,
            string imageUrl,
            bool isEnabled,
            string institutionId,
            bool isDeleted,
            string? fiscalYearId,
            BillNumberGenerationType billNumberGenerationTypeForPurchase,
            BillNumberGenerationType billNumberGenerationTypeForSales
    ) : IRequest<Result<AddCompanyResponse>>;
}
