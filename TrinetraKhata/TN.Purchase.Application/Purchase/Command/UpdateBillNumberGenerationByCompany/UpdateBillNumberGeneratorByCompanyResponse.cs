using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Authentication.Domain.Entities.Company;

namespace TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationByCompany
{
    public record UpdateBillNumberGeneratorByCompanyResponse
    (
        BillNumberGenerationType BillNumberGenerationType,
        string companyId
        );
}
