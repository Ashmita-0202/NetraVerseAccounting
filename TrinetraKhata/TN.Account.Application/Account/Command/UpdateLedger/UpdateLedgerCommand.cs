using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.UpdateLedger
{
    public record UpdateLedgerCommand
    (
        string id,
        string name,
        bool? isInventoryAffected,
        string? address,
        string? panNo,
        string? phoneNumber,
        string? maxCreditPeriod,
        string? maxDuePeriod,
        string subledgerGroupId
    ) :IRequest<Result<UpdateLedgerResponse>>;
}
