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

namespace TN.Setup.Application.Setup.Queries.District
{
    public sealed class GetAllDistrictQueryHandler : IRequestHandler<GetAllDistrictQuery, Result<PagedResult<GetAllDistrictResponse>>>
    {

        private readonly IDistrictServices _districtServices;
        private readonly IMapper _mapper;

        public GetAllDistrictQueryHandler(IDistrictServices districtServices, IMapper mapper)
        {
            _mapper = mapper;
            _districtServices = districtServices;

        }

        public async Task<Result<PagedResult<GetAllDistrictResponse>>> Handle(GetAllDistrictQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allDistrict = await _districtServices.GetAllDistrict(request.PaginationRequest, cancellationToken);
                var allDistrictDisplay = _mapper.Map<PagedResult<GetAllDistrictResponse>>(allDistrict.Data);

                return Result<PagedResult<GetAllDistrictResponse>>.Success(allDistrictDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching allDistrict", ex);
            }
        }
    }
}
