using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Account.Domain.Enum;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.AddCustomer
{
  public record AddCustomerCommand
    (
            string fullName,
            string address,
            string contact,
            string? email,
            string? description,
            string? panNo,
            int? maxDueDates,
            decimal? maxCreditLimit,
            bool? isEnabled,
            decimal? openingBalance,
            BalanceType? balanceType,
            bool? isSmsEnabled,
            bool? isEmailEnabled,
            string ledgerId
     
      ) :IRequest<Result<AddCustomerResponse>>;
}
