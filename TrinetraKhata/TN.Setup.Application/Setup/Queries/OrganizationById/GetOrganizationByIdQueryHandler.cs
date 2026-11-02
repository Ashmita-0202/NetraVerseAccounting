using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.DistrictById;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.OrganizationById
{
    public sealed class GetOrganizationByIdQueryHandler : IRequestHandler<GetOrganizationByIdQuery, Result<GetOrganizationByIdQueryResponse>>
    {
        private readonly IOrganizationServices _organizationServices;
        private readonly IMapper _mapper;

        public GetOrganizationByIdQueryHandler(IOrganizationServices organizationServices, IMapper mapper)
        {
            _organizationServices=organizationServices;
            _mapper=mapper;
        }
        public async Task<Result<GetOrganizationByIdQueryResponse>> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var organizationById = await _organizationServices.GetOrganizationById(request.id);

                return Result<GetOrganizationByIdQueryResponse>.Success(organizationById.Data);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Organization by Id", ex);

            }
        }
    }
}
