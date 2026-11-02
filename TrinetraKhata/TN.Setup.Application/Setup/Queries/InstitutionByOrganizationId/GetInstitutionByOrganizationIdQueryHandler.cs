using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.GetDistrictByProvinceId;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.InstitutionByOrganizationId
{
    public sealed class GetInstitutionByOrganizationIdQueryHandler : IRequestHandler<GetInstitutionByOrganizationIdQuery, Result<List<GetInstitutionByOrganizationIdResponse>>>
    {
        private readonly IInstitutionServices _institutionServices;
        private readonly IMapper _mapper;

        public GetInstitutionByOrganizationIdQueryHandler(IInstitutionServices institutionServices,IMapper mapper)
        {
           _institutionServices= institutionServices; 
            _mapper= mapper;

        }

        public async Task<Result<List<GetInstitutionByOrganizationIdResponse>>> Handle(GetInstitutionByOrganizationIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var institutionbyOrganizationId = await _institutionServices.GetInstitutionByOrganizationId(request.organizationId, cancellationToken);
                return Result<List<GetInstitutionByOrganizationIdResponse>>.Success(institutionbyOrganizationId.Data);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching instituion by organization Id", ex);
            }
        }
    }
}
