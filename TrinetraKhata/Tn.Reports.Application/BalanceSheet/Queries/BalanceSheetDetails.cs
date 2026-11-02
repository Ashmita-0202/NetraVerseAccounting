using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.BalanceSheet.Queries
{
    public record BalanceSheetDetails
    (
         string sectionName,         
        string masterId,
        string masterName,        
        string ledgerGroupId,
        string ledgerGroupName,    
        string subledgerGroupId,
        string subledgerGroupName,  
        string ledgerId,
        string ledgerName,          
        decimal? balance,           
         string balanceType          
    );
}
