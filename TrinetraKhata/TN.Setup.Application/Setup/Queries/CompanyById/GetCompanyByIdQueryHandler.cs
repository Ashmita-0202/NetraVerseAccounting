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

namespace TN.Setup.Application.Setup.Queries.CompanyById
{
    public sealed class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, Result<GetCompanyByIdResponse>>
    {
        private readonly ICompanyServices _companyServices;
        private readonly IMapper _mapper;

        public GetCompanyByIdQueryHandler(ICompanyServices companyServices,IMapper mapper)
        {
            _companyServices=companyServices;
            _mapper=mapper;
            
        }

        public async Task<Result<GetCompanyByIdResponse>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            try 
            {

                var companyById = await _companyServices.GetCompanyById(request.id);

                return Result<GetCompanyByIdResponse>.Success(companyById.Data);

            }
            catch (Exception ex) 
            {
                throw new Exception("An error occurred while fetching Company by Id",ex);
            
            }
        }
    }
}
