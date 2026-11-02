using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.LedgerBalance.Queries.LedgerSummary;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.VATDetails.Queries.PurchaseAndSalesVAT
{
    public class PurchaseAndSalesQueryHandler : IRequestHandler<PurchaseAndSalesVATQueries, Result<PagedResult<PurchaseAndSalesVATQueryResponse>>>
    {
        private readonly IVatDetails _vatDetails;
        private readonly IMapper _mapper;

        public PurchaseAndSalesQueryHandler(IMapper mapper, IVatDetails vatDetails)
        {
            _mapper = mapper;
            _vatDetails = vatDetails;
            
        }
        public async Task<Result<PagedResult<PurchaseAndSalesVATQueryResponse>>> Handle(PurchaseAndSalesVATQueries request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _vatDetails.GetVATReport(request);

                var vatReportResult = _mapper.Map<PagedResult<PurchaseAndSalesVATQueryResponse>>(result.Data);

                return Result<PagedResult<PurchaseAndSalesVATQueryResponse>>.Success(vatReportResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<PurchaseAndSalesVATQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
