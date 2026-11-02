using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.SubledgerGroupById
{
    public record  GetSubledgerGroupByIdResponse
    (
        
        string name,
        string ledgerGroupId,
       bool isSeeded
    );
}
