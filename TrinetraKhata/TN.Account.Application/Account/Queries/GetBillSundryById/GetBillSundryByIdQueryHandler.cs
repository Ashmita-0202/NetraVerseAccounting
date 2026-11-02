using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.GetBillSundryById
{
    public  class GetBillSundryByIdQueryHandler:IRequestHandler<GetBillSundryByIdQuery,Result<GetBillSundryByIdQueryResponse>>
    {
        private readonly IBillSundryServices _billSundryServices;
        private readonly IMapper _mapper;

        public GetBillSundryByIdQueryHandler(IBillSundryServices billSundryServices,IMapper mapper)
        {
            _billSundryServices=billSundryServices;
            _mapper=mapper;
        }

        public async Task<Result<GetBillSundryByIdQueryResponse>> Handle(GetBillSundryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var billSundryById = await _billSundryServices.GetSundryBillById(request.Id);
                return Result<GetBillSundryByIdQueryResponse>.Success(billSundryById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Sundry Bill by using id", ex);
            }
        }
    }
}
