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

namespace TN.Setup.Application.Setup.Queries.GetCompanyDetailsByCompanyId
{
    public  class GetCompanyDetailsByCompanyIdQueryHandler:IRequestHandler<GetCompanyDetailsByCompanyIdQuery, Result<List<GetCompanyDetailsByCompanyIdQueryResponse>>>
    {
        private readonly ICompanyServices _companyServices;
        private readonly IMapper _mapper;

        public GetCompanyDetailsByCompanyIdQueryHandler(ICompanyServices companyServices,IMapper mapper)
        {
            _companyServices=companyServices;
            _mapper=mapper;
        }

        public async Task<Result<List<GetCompanyDetailsByCompanyIdQueryResponse>>> Handle(GetCompanyDetailsByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var companyDetails = await _companyServices.GetCompanyDetailsByInstitutionId(request.institutionId, cancellationToken);
                return Result<List<GetCompanyDetailsByCompanyIdQueryResponse>>.Success(companyDetails.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching company details", ex);
            }
        }
    }
}
