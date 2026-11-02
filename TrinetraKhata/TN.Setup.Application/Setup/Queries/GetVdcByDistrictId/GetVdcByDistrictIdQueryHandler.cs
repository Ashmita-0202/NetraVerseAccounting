using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.GetVdcByDistrictId
{
    public sealed class GetVdcByDistrictIdQueryHandler : IRequestHandler<GetVdcByDistrictIdQuery, Result<List<GetVdcByDistrictIdResponse>>>
    {
        
        private readonly IVdcServices _Services;
        private readonly IMapper _mapper;

        public GetVdcByDistrictIdQueryHandler(IVdcServices vdcServices, IMapper mapper)
        {
            _Services=vdcServices;
            _mapper = mapper;
        }

        public async Task<Result<List<GetVdcByDistrictIdResponse>>> Handle(GetVdcByDistrictIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var vdcByDistrictId = await _Services.GetVdcByDistrictId(request.Id, cancellationToken);
                return Result<List<GetVdcByDistrictIdResponse>>.Success(vdcByDistrictId.Data);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching VDC by DistrictId",ex);
            };
        }
    }
}
