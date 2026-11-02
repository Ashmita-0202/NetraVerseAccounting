using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.GetMunicipalityByDistrictId
{
    public sealed class GetMunicipalityByDistrictIdQueryHandler : IRequestHandler<GetMunicipalityByDistrictIdQuery, Result<List<GetMunicipalityByDistrictIdResponse>>>{
        private readonly IMunicipalityServices _Services;
        private readonly IMapper _mapper;
        

        public GetMunicipalityByDistrictIdQueryHandler(IMunicipalityServices municipalityServices,IMapper mapper)
        {
            _Services=municipalityServices;
            _mapper = mapper;
        }

        public async Task<Result<List<GetMunicipalityByDistrictIdResponse>>> Handle(GetMunicipalityByDistrictIdQuery request, CancellationToken cancellationToken)
        {
            try
            { 
                var municipalityByDistrictId= await _Services.GetMunicipalityByDistrictId(request.Id,cancellationToken);
                return Result<List<GetMunicipalityByDistrictIdResponse>>.Success(municipalityByDistrictId.Data);

            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching the Municipality by District",ex);

            }

        }
    }
}