using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Authentication.Domain.Entities.Company;

namespace TN.Setup.Application.Setup.Command.AddCompany
{
    public record AddCompanyResponse
    (
          
            string name="",
            string address = "" ,
            string shortName = "",
            string email="",
            string contactNumber = "",
            string contactPerson = "",
            string pan="",
            string imageUrl = "",
            bool isEnabled = true,
            string institutionId = "",
            DateTime createdDate =default,
            string createdBy = "",
            DateTime modifiedDate = default,
            string modifiedBy = "",
            bool isDeleted = true,
            string fiscalYearId="",
            BillNumberGenerationType billNumberGenerationTypeForPurchase= default,
            BillNumberGenerationType BillNumberGenerationTypeForSales = default

    );
}
