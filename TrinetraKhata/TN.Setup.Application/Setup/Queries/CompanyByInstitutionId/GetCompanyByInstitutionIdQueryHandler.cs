using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.InstitutionByOrganizationId;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.CompanyByInstitutionId
{
    public sealed class GetCompanyByInstitutionIdQueryHandler : IRequestHandler<GetCompanyByInstitutionIdQuery, Result<List<GetCompanyByInstitutionIdResponse>>>
    {
        private readonly ICompanyServices _companyServices;
        private readonly IMapper _mapper;


        public GetCompanyByInstitutionIdQueryHandler(ICompanyServices companyServices,IMapper mapper)
        {
            _companyServices=companyServices;
            _mapper=mapper;
            
        }

        public async Task<Result<List<GetCompanyByInstitutionIdResponse>>> Handle(GetCompanyByInstitutionIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var companyByInstitutionId = await _companyServices.GetCompanyByInstitutionId(request.institutionId, cancellationToken);
                return Result<List<GetCompanyByInstitutionIdResponse>>.Success(companyByInstitutionId.Data);


            }
            catch (Exception ex) 
            {
              throw new Exception("An error occurred while fetching a company by institution id",ex);
            
            }
        }
    }
}
