using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.CloseFiscalYear
{
    public record CloseFiscalYearCommand
    (
        string closedFiscalId,
        bool autoOpenNext,
        bool? generateClosingEntries

        ) : IRequest<Result<CloseFiscalYearResponse>>;
    
}
