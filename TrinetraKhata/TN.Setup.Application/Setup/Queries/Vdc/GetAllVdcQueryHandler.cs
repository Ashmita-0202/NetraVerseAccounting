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

namespace TN.Setup.Application.Setup.Queries.Vdc
{
    public sealed class GetAllVdcQueryHandler : IRequestHandler<GetAllVdcQuery, Result<PagedResult<GetAllVdcResponse>>>{
        private readonly IVdcServices _Services;
        private readonly IMapper _mapper;

        public GetAllVdcQueryHandler(IVdcServices vdcServices,IMapper mapper)
        {
            _Services=vdcServices;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<GetAllVdcResponse>>> Handle(GetAllVdcQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allVdc = await _Services.GetAllVdc(request.paginationRequest, cancellationToken);
                var allVdcDisplay = _mapper.Map<PagedResult<GetAllVdcResponse>>(allVdc.Data);
                return Result<PagedResult<GetAllVdcResponse>>.Success(allVdcDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occur while Fetching allVdc", ex);
            }
        }
    }
}
