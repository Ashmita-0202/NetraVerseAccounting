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

namespace TN.Setup.Application.Setup.Queries.DistrictById
{
    public sealed class GetDistrictByIdQueryHandler : IRequestHandler<GetDistrictByIdQuery, Result<GetDistrictByIdResponse>>
    {
        private readonly IDistrictServices _services;
        private readonly IMapper _mapper;

        public GetDistrictByIdQueryHandler(IDistrictServices districtServices, IMapper mapper)
        {
            _services = districtServices;
            _mapper = mapper;

        }
        public async Task<Result<GetDistrictByIdResponse>> Handle(GetDistrictByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var districtById = await _services.GetDistrictById(request.Id);

                return Result<GetDistrictByIdResponse>.Success(districtById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching district by Id", ex);
            }
        }
    }
}
