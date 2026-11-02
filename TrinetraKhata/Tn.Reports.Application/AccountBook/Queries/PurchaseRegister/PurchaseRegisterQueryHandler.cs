using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.AccountBook.Queries.SalesRegister;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.AccountBook.Queries.PurchaseRegister
{
    public sealed class PurchaseRegisterQueryHandler : IRequestHandler<PurchaseRegisterQueries, Result<PagedResult<PurchaseRegisterQueryResponse>>>
    {
        private readonly IAccountBookServices _accountBookServices;
        private readonly IMapper _mapper;

        public PurchaseRegisterQueryHandler(IAccountBookServices accountBookServices, IMapper mapper)
        {
            _accountBookServices = accountBookServices;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<PurchaseRegisterQueryResponse>>> Handle(PurchaseRegisterQueries request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _accountBookServices.GetPurchaseRegister(request.PaginationRequest, request.PurchaseRegisterDTOs);

                var purchaseRegisterResponse = _mapper.Map<PagedResult<PurchaseRegisterQueryResponse>>(result.Data);

                return Result<PagedResult<PurchaseRegisterQueryResponse>>.Success(purchaseRegisterResponse);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<PurchaseRegisterQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
