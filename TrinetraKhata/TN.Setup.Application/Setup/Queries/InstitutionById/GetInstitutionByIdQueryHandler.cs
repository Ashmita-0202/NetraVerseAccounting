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

namespace TN.Setup.Application.Setup.Queries.InstitutionById
{
    public sealed class GetInstitutionByIdQueryHandler : IRequestHandler<GetInstitutionByIdQuery, Result<GetInstitutionByIdResponse>>
    {
        private readonly IInstitutionServices _institutionServices;
        private readonly IMapper _mapper;

        public GetInstitutionByIdQueryHandler(IInstitutionServices institutionServices, IMapper mapper) 
        {
         _institutionServices=institutionServices;
            _mapper=mapper;


        }

        public async Task<Result<GetInstitutionByIdResponse>> Handle(GetInstitutionByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var institutionById = await _institutionServices.GetInstitutionById(request.Id);

                return Result<GetInstitutionByIdResponse>.Success(institutionById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching Institution by Id", ex);
            
            
            }
        }
    }
}
