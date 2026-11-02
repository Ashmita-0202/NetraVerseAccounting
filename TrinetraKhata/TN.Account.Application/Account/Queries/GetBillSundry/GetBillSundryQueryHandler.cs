using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.GetBillSundry
{
    public  class GetBillSundryQueryHandler:IRequestHandler<GetBillSundryQuery,Result<PagedResult<GetBillSundryQueryResponse>>>
    {
        private readonly IBillSundryServices _billSundryServices;
        private readonly IMapper _mapper;

        public GetBillSundryQueryHandler(IBillSundryServices billSundryServices,IMapper mapper)
        {
            _billSundryServices = billSundryServices;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetBillSundryQueryResponse>>> Handle(GetBillSundryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allLedger = await _billSundryServices.GetSundryBill(request.PaginationRequest, cancellationToken);
                var allLedgerDisplay = _mapper.Map<PagedResult<GetBillSundryQueryResponse>>(allLedger.Data);
                return Result<PagedResult<GetBillSundryQueryResponse>>.Success(allLedgerDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all Sundry Bill", ex);
            }
        }
    }
}
