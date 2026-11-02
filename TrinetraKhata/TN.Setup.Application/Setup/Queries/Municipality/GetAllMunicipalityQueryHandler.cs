using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.Province;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.Municipality
{
    public sealed class GetAllMunicipalityQueryHandler : IRequestHandler<GetAllMunicipalityQuery, Result<PagedResult<GetAllMunicipalityResponse>>>
    {
        private readonly IMunicipalityServices _municipalityServices;
        private readonly IMapper _mapper;


        public GetAllMunicipalityQueryHandler(IMunicipalityServices municipalityServices, IMapper mapper)
        {
            _municipalityServices = municipalityServices;
            _mapper = mapper;

        }


        public async Task<Result<PagedResult<GetAllMunicipalityResponse>>> Handle(GetAllMunicipalityQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allMunicipality = await _municipalityServices.GetAllMunicipality(request.PaginationRequest, cancellationToken);
                var allMunicipalityDisplay = _mapper.Map<PagedResult<GetAllMunicipalityResponse>>(allMunicipality.Data);

                return Result<PagedResult<GetAllMunicipalityResponse>>.Success(allMunicipalityDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching allMunicpality", ex);
            }
        }

    }
}
