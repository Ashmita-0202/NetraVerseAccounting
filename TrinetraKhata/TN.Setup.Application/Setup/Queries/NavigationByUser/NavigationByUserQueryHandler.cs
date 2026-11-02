using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.NavigationByUser
{
    public class NavigationByUserQueryHandler : IRequestHandler<NavigationByUserQuery, Result<List<NavigationByUserResponse>>>
    {
        private readonly IModule _module;

        public NavigationByUserQueryHandler(IModule module)
        {
            _module = module;

        }
        public async Task<Result<List<NavigationByUserResponse>>> Handle(NavigationByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var navigationByUser = await _module.GetNavigationMenuByUser(request.id);
                if (navigationByUser is null)
                {
                    return Result<List<NavigationByUserResponse>>.Failure("NotFound", "Modules does not Found");
                }

                return Result<List<NavigationByUserResponse>>.Success(navigationByUser.Data);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Navigation by {request.id}", ex);
            }
        }
    }
}
