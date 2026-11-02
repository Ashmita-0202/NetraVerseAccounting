using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.District;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.Company
{
    public sealed class GetAllCompanyQueryHandler : IRequestHandler<GetAllCompanyQuery, Result<PagedResult<GetAllCompanyQueryResponse>>>
    {
        private readonly ICompanyServices _companyServices;
        private readonly IMapper _mapper;

        public GetAllCompanyQueryHandler(ICompanyServices comapnyServices,IMapper mapper) 
        { 
        
        _companyServices= comapnyServices;
            _mapper= mapper;
        }
        public async Task<Result<PagedResult<GetAllCompanyQueryResponse>>> Handle(GetAllCompanyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allComapny = await _companyServices.GetAllCompany(request.PaginationRequest, cancellationToken);
                var allCompanyDisplay = _mapper.Map<PagedResult<GetAllCompanyQueryResponse>>(allComapny.Data);

                return Result<PagedResult<GetAllCompanyQueryResponse>>.Success(allCompanyDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all company", ex);
            }
        }
    }
}
