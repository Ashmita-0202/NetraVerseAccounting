using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.TradingAccount
{
    public class GetTradingAccountQueryHandler:IRequestHandler<GetTradingAccountQuery,Result<GetTradingAccountQueryResponse>>
    {
        private readonly ITradingServices _tradingServices;
        private readonly IMapper _mapper;

        public GetTradingAccountQueryHandler(ITradingServices tradingServices,IMapper mapper) 
        {
            _tradingServices= tradingServices;
            _mapper= mapper;
        
        }

        public async Task<Result<GetTradingAccountQueryResponse>> Handle(GetTradingAccountQuery request, CancellationToken cancellationToken)
        {

            try
            {

                var result = await _tradingServices.GenerateTradingReport(request.startDate, request.endDate,request.companyId, cancellationToken);
                return Result<GetTradingAccountQueryResponse>.Success(result);
            }
            catch (Exception ex)
            {
                throw new Exception("AN errorr occurred while generating trading report", ex);
            }
        }
    }
}
