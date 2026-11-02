using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Authentication.Domain.Entities.Company;

namespace TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationByCompany
{
   public record UpdateBillNumberGenerationByCompanyRequest
    (
       BillNumberGenerationType BillNumberGenerationType
        //string companyId

     );
}
