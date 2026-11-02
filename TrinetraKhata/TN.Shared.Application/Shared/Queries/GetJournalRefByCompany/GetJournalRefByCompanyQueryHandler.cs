using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Queries.GetPaymentTransactionNumberType;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetJournalRefByCompany
{
    public  class GetJournalRefByCompanyQueryHandler:IRequestHandler<GetJournalRefByCompanyQuery, Result<GetJournalRefByCompanyQueryResponse>>
    
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public GetJournalRefByCompanyQueryHandler(ISettingServices settingServices, IMapper mapper)
        {
            _settingServices=settingServices;
            _mapper=mapper;
        }

        public async Task<Result<GetJournalRefByCompanyQueryResponse>> Handle(GetJournalRefByCompanyQuery request, CancellationToken cancellationToken)
        {

            var journalRef = await _settingServices.GetJournalRefByCompany(request.companyId, cancellationToken);

            if (journalRef is not { IsSuccess: true, Data: not null })
                return Result<GetJournalRefByCompanyQueryResponse>.Failure(journalRef?.Message ?? $"Getting Journal Reference Number by company {request.companyId}");

            return Result<GetJournalRefByCompanyQueryResponse>.Success(
                _mapper.Map<GetJournalRefByCompanyQueryResponse>(journalRef.Data)
            );
        }
    }
}
