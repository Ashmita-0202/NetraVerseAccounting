using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.Company;

namespace TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationByCompany
{
   public record UpdateBillNumberGenerationByCompanyCommand
    (
        BillNumberGenerationType BillNumberGenerationType,
        string companyId

   ):IRequest<Result<UpdateBillNumberGenerationByCompanyResponse>>;
}
