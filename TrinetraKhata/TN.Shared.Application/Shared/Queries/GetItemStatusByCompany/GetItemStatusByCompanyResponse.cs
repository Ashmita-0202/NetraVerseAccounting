using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Queries.GetExpiredDateItemStatusByCompany
{
    public record GetItemStatusByCompanyResponse
    (
        bool isExpiredDate,
        bool isSerialNo
        );
}
