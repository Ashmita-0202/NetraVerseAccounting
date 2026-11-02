using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.CompanySettings;

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType
{
    public record  UpdatePurchaseQuotationTypeCommand
    (

        string companyId,
        PurchaseSalesQuotationNumberType purchaseQuotationNumberType


    ) :IRequest<Result<UpdatePurchaseQuotationTypeResponse>>;
}
