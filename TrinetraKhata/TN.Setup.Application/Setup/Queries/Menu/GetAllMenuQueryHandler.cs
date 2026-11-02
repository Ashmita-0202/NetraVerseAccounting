using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.Institution;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.Menu
{
    public sealed class GetAllMenuQueryHandler:IRequestHandler<GetAllMenuQuery,Result<PagedResult<GetAllMenuResponse>>>
    {
        private readonly IMenuServices _menuServices;
        private readonly IMapper _mapper;

        public GetAllMenuQueryHandler(IMenuServices menuServices,IMapper mapper)
        {
            _menuServices=menuServices;
            _mapper=mapper;
        
        }

        public async Task<Result<PagedResult<GetAllMenuResponse>>> Handle(GetAllMenuQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allMenu = await _menuServices.GetAllMenu(request.PaginationRequest, cancellationToken);
                var allMenuDisplay = _mapper.Map<PagedResult<GetAllMenuResponse>>(allMenu.Data);

                return Result<PagedResult<GetAllMenuResponse>>.Success(allMenuDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching allMenu", ex);
            }
        }
    }
}
