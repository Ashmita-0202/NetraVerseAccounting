using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Command.Initialize;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.ServiceInterface
{
    public interface IInitializeServices
    {
        Task<Result<InitializeCommandResponse>> InitializeAsync();
    }
}
