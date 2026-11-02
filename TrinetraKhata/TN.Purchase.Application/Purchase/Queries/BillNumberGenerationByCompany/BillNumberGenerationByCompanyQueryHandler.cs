using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Queries.Purchase;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Purchase.Application.Purchase.Queries.BillNumberGenerationByCompany
{
    public class BillNumberGenerationByCompanyQueryHandler : IRequestHandler<BillNumberGenerationByCompanyQueries, Result<BillNumberGenerationByCompanyQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;

        public BillNumberGenerationByCompanyQueryHandler(IMapper mapper, IPurchaseDetailsServices purchaseDetailsServices)
        {
            _mapper = mapper;
            _purchaseDetailsServices = purchaseDetailsServices;

        }
        public async Task<Result<BillNumberGenerationByCompanyQueryResponse>> Handle(BillNumberGenerationByCompanyQueries request, CancellationToken cancellationToken)
        {
            try
            {
                var allPurchaseDetails = await _purchaseDetailsServices.GetBillNumberStatusByCompany(request.id, cancellationToken);

                if (allPurchaseDetails == null)
                {
                    return Result<BillNumberGenerationByCompanyQueryResponse>.Failure("Purchase details not found.");
                }

                return Result<BillNumberGenerationByCompanyQueryResponse>.Success(allPurchaseDetails.Data);


            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all purchaseDetails ", ex);
            }
        }
    }
}
