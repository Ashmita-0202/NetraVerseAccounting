using AutoMapper;
using MediatR;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.MenuStatusBySubModulesandRolesId;
using TN.Setup.Application.Setup.Queries.Modules;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.MenuStatusBySubModulesAndRolesId
{
    public sealed class MenuStatusBySubModulesAndRolesIdQueryHandler : IRequestHandler<MenuStatusBySubModulesAndRolesIdQuery, Result<List<MenuStatusBySubModulesAndRolesIdResponse>>>
    {
        private readonly IMenuServices _menuServices;
        private readonly IMapper _mapper;

        public MenuStatusBySubModulesAndRolesIdQueryHandler(IMenuServices menuServices, IMapper mapper)
        {
            _menuServices = menuServices;
            _mapper = mapper;
        }

        public async Task<Result<List<MenuStatusBySubModulesAndRolesIdResponse>>> Handle(MenuStatusBySubModulesAndRolesIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getMenuStatus = await _menuServices.GetMenuStatusBySubModulesAndRoles(request.subModulesId, request.rolesId, cancellationToken);
                var allMenuStatusDisplay = _mapper.Map<List<MenuStatusBySubModulesAndRolesIdResponse>>(getMenuStatus.Data);

                return Result<List<MenuStatusBySubModulesAndRolesIdResponse>>.Success(allMenuStatusDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all company", ex);
            }
        }
    }
}
