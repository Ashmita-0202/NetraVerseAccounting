using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.ProvinceById;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.MunicipalityById
{
    public sealed class GetMunicipalityByIdQueryHandler : IRequestHandler<GetMunicipalityByIdQuery, Result<GetMunicipalityByIdResponse>>
    {
        private readonly IMunicipalityServices _services;
        private readonly IMapper _mapper;

        public GetMunicipalityByIdQueryHandler(IMunicipalityServices municipalityServices, IMapper mapper)
        {
            _services = municipalityServices;
            _mapper = mapper;

        }
        public async Task<Result<GetMunicipalityByIdResponse>> Handle(GetMunicipalityByIdQuery request, CancellationToken cancellationToken)
        {

            try {
                var municipalityById = await _services.GetMunicipalityById(request.Id);

                return Result<GetMunicipalityByIdResponse>.Success(municipalityById.Data);
            } catch(Exception ex) {
            
                throw new Exception("An error occurred while fetching municipality by Id", ex);
            }
        }



    }
}
