using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.Initialize
{
    public record InitializeCommandResponse
    (
        string username,
        string password,
        string message
        );
}
