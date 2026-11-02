using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.District;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.Institution
{
    public sealed class GetAllInstitutionQueryHandler : IRequestHandler<GetAllInstitutionQuery, Result<PagedResult<GetAllInstitutionResponse>>>
    {
        private readonly IInstitutionServices _institutionServices;
        private readonly IMapper _mapper;

        public GetAllInstitutionQueryHandler(IInstitutionServices institutionServices,IMapper mapper)
        {
            _institutionServices=institutionServices;
            _mapper=mapper;
        }
        public async Task<Result<PagedResult<GetAllInstitutionResponse>>> Handle(GetAllInstitutionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allInstitution = await _institutionServices.GetAllInstitution(request.PaginationRequest, cancellationToken);
                var allInstitutionDisplay = _mapper.Map<PagedResult<GetAllInstitutionResponse>>(allInstitution.Data);

                return Result<PagedResult<GetAllInstitutionResponse>>.Success(allInstitutionDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching allInstitution", ex);
            }
        }
    }
}
