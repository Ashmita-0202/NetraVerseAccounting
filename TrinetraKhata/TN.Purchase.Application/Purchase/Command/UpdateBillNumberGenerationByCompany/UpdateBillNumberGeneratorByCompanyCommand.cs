using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.Company;

namespace TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationByCompany
{
    public record UpdateBillNumberGeneratorByCompanyCommand
    (
        BillNumberGenerationType BillNumberGenerationType,
        string companyId
        ) : IRequest<Result<UpdateBillNumberGeneratorByCompanyResponse>>;
}
