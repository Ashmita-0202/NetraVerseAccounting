using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NV.Payment.Domain.Entities.PaymentMethod;

namespace NV.Payment.Application.Payment.Queries.FilterPaymentMethod
{
    public record  GetFilterPaymentMethodResponse
    (
            string id = "",
            string name = "",
            string SubledgerGroupsId = ""
            //PaymentType type = default

    );
}
