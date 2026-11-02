using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NV.Payment.Application.Payment.Queries.FilterPaymentMethod
{
    public record  FilterPaymentMethodDto
    (
        string? name,
        string? startDate,
        string? endDate

    );
}
