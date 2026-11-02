using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.GetDistrictByProvinceId
{
    public sealed class GetDistrictByProvinceIdQueryHandler : IRequestHandler<GetDistrictByProvinceIdQuery, Result<List<GetDistrictByProvinceIdResponse>>>
    {
        private readonly IDistrictServices _services;
        private readonly IMapper _mapper;

        public GetDistrictByProvinceIdQueryHandler(IDistrictServices districtServices, IMapper mapper)
        {
            _services = districtServices;
            _mapper = mapper;
            
        }
        public async Task<Result<List<GetDistrictByProvinceIdResponse>>> Handle(GetDistrictByProvinceIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var districtbyProvinceId = await _services.GetDistrictByProvinceId(request.Id, cancellationToken);
                return Result<List<GetDistrictByProvinceIdResponse>>.Success(districtbyProvinceId.Data);

            }catch(Exception ex)
            {
                throw new Exception("An error occured while getching district by province", ex);
            }
        }
    }
}
