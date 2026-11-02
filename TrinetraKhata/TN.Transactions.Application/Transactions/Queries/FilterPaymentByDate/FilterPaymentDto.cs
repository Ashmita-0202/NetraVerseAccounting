using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Queries.FilterPaymentByDate
{
    public record  FilterPaymentDto
    (
         string? ledgerId,
         string? startDate,
         string? endDate
    );
}
