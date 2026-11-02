using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TN.Shared.Infrastructure.CustomMiddleware.GlobalErrorHandling
{
    public record ErrorDetails
(
    int StatusCode,
    string? Message
)
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }

}
