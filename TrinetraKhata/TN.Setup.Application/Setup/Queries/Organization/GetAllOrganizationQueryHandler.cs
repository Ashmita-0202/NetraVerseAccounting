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

namespace TN.Setup.Application.Setup.Queries.Organization
{
    public sealed class GetAllOrganizationQueryHandler : IRequestHandler<GetAllOrganizationQuery, Result<PagedResult<GetAllOrganizationResponse>>>
    {
        private readonly IOrganizationServices _organizationServices;
        private readonly IMapper _mapper;

        public GetAllOrganizationQueryHandler(IOrganizationServices organizationServices, IMapper mapper)
        { 
        
        _organizationServices=organizationServices;
            _mapper=mapper;
        }
        public async Task<Result<PagedResult<GetAllOrganizationResponse>>> Handle(GetAllOrganizationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allOrganization = await _organizationServices.GetAllOrganization(request.PaginationRequest, cancellationToken);
                var allOrganizationDisplay = _mapper.Map<PagedResult<GetAllOrganizationResponse>>(allOrganization.Data);

                return Result<PagedResult<GetAllOrganizationResponse>>.Success(allOrganizationDisplay);


            }
            catch (Exception ex)
            {
            
                throw new Exception("An error occurred while fetching",ex);
            }

        }
    }
}
