using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.GetDistrictByProvinceId;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.GetMenuBySubModulesId
{
    public class GetMenuBySubModulesIdQueryHandler : IRequestHandler<GetMenuBySubModulesIdQuery, Result<List<GetMenuBySubModulesIdResponse>>>
    {
        private readonly IMenuServices _menuServices;

        public GetMenuBySubModulesIdQueryHandler(IMenuServices menuServices)
        {
            _menuServices = menuServices;
            
        }
        public async Task<Result<List<GetMenuBySubModulesIdResponse>>> Handle(GetMenuBySubModulesIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getmenuBySubModulesId = await _menuServices.GetMenusBySubModulesId(request.submodulesId, cancellationToken);
                return Result<List<GetMenuBySubModulesIdResponse>>.Success(getmenuBySubModulesId.Data);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting menu by {request.submodulesId}", ex);
            }
        }
    }
}
