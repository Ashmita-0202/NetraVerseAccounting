using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;


namespace TN.Account.Application.Account.Command.AddLedgerGroup
{
    public record AddLedgerGroupRequest
        (
            string name,
            bool isCustom,
            string masterId,
            bool isPrimary
        );
   
}
