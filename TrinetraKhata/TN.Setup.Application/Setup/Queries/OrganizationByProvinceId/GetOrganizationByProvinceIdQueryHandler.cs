using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.GetDistrictByProvinceId;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.GetOrganizationByProvinceId
{
    public sealed class GetOrganizationByProvinceIdQueryHandler : IRequestHandler<GetOrganizationByProvinceIdQuery, Result<List<GetOrganizationByProvinceIdResponse>>>
    {
        private readonly IOrganizationServices _organizationServices;
        private readonly IMapper _mapper;

        public GetOrganizationByProvinceIdQueryHandler(IOrganizationServices organizationServices,IMapper mapper)
        {
            _organizationServices = organizationServices;
            _mapper = mapper;
        }
        public async Task<Result<List<GetOrganizationByProvinceIdResponse>>> Handle(GetOrganizationByProvinceIdQuery request, CancellationToken cancellationToken)
        {
            try 
            {
                var organizationByProvinceId = await _organizationServices.GetOrganizationByProvinceId(request.id, cancellationToken);
                return Result<List<GetOrganizationByProvinceIdResponse>>.Success(organizationByProvinceId.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An errror occurred while fetching Organization by province Id", ex);
            }
        }
    }

}
