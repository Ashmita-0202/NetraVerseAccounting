using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberByCompany
{
    public record  UpdateSalesReferenceNumberCommand
    (
        bool showReferenceNumberForSales,
         string companyId
    ):IRequest<Result<UpdateSalesReferenceNumberResponse>>;
}
