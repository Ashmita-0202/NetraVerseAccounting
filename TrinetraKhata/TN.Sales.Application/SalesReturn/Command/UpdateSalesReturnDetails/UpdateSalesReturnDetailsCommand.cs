using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Sales.Application.SalesReturn.Command.UpdateSalesReturnDetails
{
    public record class UpdateSalesReturnDetailsCommand
    (

         string id,
         string salesDetailsId,
         DateTime returnDate,
         decimal totalReturnAmount,
         decimal taxAdjustment,
         decimal netReturnAmount,
         string reason,
         string createdBy,
         DateTime createdAt,
         string modifiedBy,
         DateTime modifiedAt
    ) :IRequest<Result<UpdateSalesReturnDetailsResponse>>;
}
