using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.Shared.Command.UpdateItemStatusByCompany;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdateExpiredDateItemStatusByCompany
{
    public record UpdateItemStatusByCompanyCommand
    (
        string companyId,
        bool isExpiredDate,
        bool isSerialNo
        ) : IRequest<Result<UpdateItemStatusByCompanyResponse>>;
}
