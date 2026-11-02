using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Account.Application.Account.Command.AddLedgerGroup;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.AddLedger
{
    public record AddLedgerCommand
    (

             string name,
            bool? isInventoryAffected,
            string? address,
            string? panNo,
            string? phoneNumber,
            string? maxCreditPeriod,
            string? maxDuePeriod,
            string subledgerGroupId,
            decimal? openingBalance
  
        ) : IRequest<Result<AddLedgerResponse>>;
}
