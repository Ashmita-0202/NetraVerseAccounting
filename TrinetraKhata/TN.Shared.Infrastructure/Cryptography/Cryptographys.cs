using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.ICryptography;

namespace TN.Shared.Infrastructure.Cryptography
{
    public class Cryptographys : ICryptography
    {
        public Task<string> Base64UrlDecoder(string value)
        {
            var decodeBytes = WebEncoders.Base64UrlDecode(value);
            var decodedValue = Encoding.UTF8.GetString(decodeBytes);
            return Task.FromResult(decodedValue);
        }

        public Task<string> Base64UrlEncoder(string value)
        {
            var encodedValue = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(value));
            return Task.FromResult(encodedValue);
        }
    }
}
