using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Authentication.Domain.Entities.Company;

namespace TN.Setup.Application.Setup.Queries.CompanyByInstitutionId
{
    public record GetCompanyByInstitutionIdResponse
        (
            string id,
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
            DateTime createdDate,
            string createdBy,
            DateTime modifiedDate,
            string modifiedBy,
            bool isDeleted,
             BillNumberGenerationType billNumberGenerationTypeForPurchase,
            BillNumberGenerationType billNumberGenerationTypeForSales
        );
    
}
