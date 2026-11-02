using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.IRepository;

namespace TN.Shared.Infrastructure.Repository
{
    public class SerialNumberGenerator : ISerialNumberGenerator
    {
        public string GenerateSerialNumber()
        {
            return string.Empty;

            //return $"SN-{Guid.NewGuid():N}".Substring(0, 12);
        }
    }
}
