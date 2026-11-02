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

namespace TN.Setup.Application.Setup.Queries.VdcById
{
    public sealed class GetVdcByIdQueryHandler : IRequestHandler<GetVdcByIdQuery, Result<GetVdcByIdResponse>>
    {

        private readonly IVdcServices _services;
        private readonly IMapper _mapper;

        public GetVdcByIdQueryHandler(IVdcServices vdcServices, IMapper mapper)
        {
            _services = vdcServices;
            _mapper = mapper;

        }



        public async Task<Result<GetVdcByIdResponse>> Handle(GetVdcByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var vdcById = await _services.GetVdcById(request.Id);
                return Result<GetVdcByIdResponse>.Success(vdcById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occur while fetching Vdc by Id", ex);
            }
        }
    }
}
