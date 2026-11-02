using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.ICryptography
{
    public interface ICryptography
    {
        Task<string> Base64UrlEncoder(string value);
        Task<string> Base64UrlDecoder(string value);
    }
}
