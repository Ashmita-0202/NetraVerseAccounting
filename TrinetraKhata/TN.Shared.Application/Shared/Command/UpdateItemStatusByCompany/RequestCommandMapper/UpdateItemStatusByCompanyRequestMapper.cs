using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.Shared.Command.UpdateItemStatusByCompany;

namespace TN.Shared.Application.Shared.Command.UpdateExpiredDateItemStatusByCompany.RequestCommandMapper
{
    public static class UpdateItemStatusByCompanyRequestMapper
    {
        public static UpdateItemStatusByCompanyCommand ToCommand(this UpdateItemStatusByCompanyRequest request, string id)
        {
            return new UpdateItemStatusByCompanyCommand
                (
                id,
                request.isExpiredDate,
                request.isSerialNo
                );
            
        }
    }
}
