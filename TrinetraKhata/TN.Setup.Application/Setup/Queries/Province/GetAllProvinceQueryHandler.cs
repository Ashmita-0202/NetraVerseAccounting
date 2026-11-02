using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.Province
{
    public sealed class GetAllProvinceQueryHandler : IRequestHandler<GetAllProvinceQuery, Result<PagedResult<GetAllProvinceResponse>>>
    {

        private readonly IProvinceServices _provinceServices;
        private readonly IMapper _mapper;

        public GetAllProvinceQueryHandler(IProvinceServices provinceServices, IMapper mapper)
        {
            _mapper = mapper;
            _provinceServices = provinceServices;
            
        }

        public async Task<Result<PagedResult<GetAllProvinceResponse>>> Handle(GetAllProvinceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allProvince = await _provinceServices.GetAllProvince(request.PaginationRequest, cancellationToken);
                var allProvinceDisplay = _mapper.Map<PagedResult<GetAllProvinceResponse>>(allProvince.Data);

                return Result<PagedResult<GetAllProvinceResponse>>.Success(allProvinceDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching allProvince", ex);
            }
        }
    }
}
