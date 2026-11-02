using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Reports.Application.ItemwisePurchaseReport
{
    public record  ItemwisePurchaseReportDto
    (

        string? startDate,
        string? endDate,
        string? stockCenterId,
        string? itemId,
        string? itemGroupId,
        string? ledgerId

    );
}
