using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.IRepository
{
    public interface ITokenService
    {
        string GetUserId();
        string GetRole();
        string GetUsername();
        List<string> CompanyId();
        string InstitutionId();

        Task<bool> isDemoUser();
    }
}
