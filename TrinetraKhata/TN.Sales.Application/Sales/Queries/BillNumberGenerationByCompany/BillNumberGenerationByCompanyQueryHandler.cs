using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Sales.Application.Sales.Queries.BillNumberGenerationByCompany
{
 public class BillNumberGenerationByCompanyQueryHandler:IRequestHandler<BIllNumberGenerationByCompanyQuery, Result<BIllNumberGenerationByCompanyQueryResponse>>
    {
        private readonly ISalesDetailsServices _salesDetailsServices;
        private readonly IMapper _mapper;

        public BillNumberGenerationByCompanyQueryHandler(ISalesDetailsServices salesDetailsServices,IMapper mapper)
        {
            _salesDetailsServices = salesDetailsServices;
                _mapper=mapper;
        }

        public async  Task<Result<BIllNumberGenerationByCompanyQueryResponse>> Handle(BIllNumberGenerationByCompanyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allSalesDetails = await _salesDetailsServices.GetBillNumberStatusByCompany(request.id, cancellationToken);

                if (allSalesDetails == null)
                {
                    return Result<BIllNumberGenerationByCompanyQueryResponse>.Failure("sales details not found.");
                }

                return Result<BIllNumberGenerationByCompanyQueryResponse>.Success(allSalesDetails.Data);


            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching billNumber status by company ", ex);
            }
        }
    }
}
