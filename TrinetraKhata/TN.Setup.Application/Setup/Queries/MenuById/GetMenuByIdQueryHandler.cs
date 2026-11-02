using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.GetSubModulesById;
using TN.Setup.Domain.Entities;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.MenuById
{
    public record GetMenuByIdQueryHandler :IRequestHandler<GetMenuByIdQuery,Result<GetMenuByIdResponse>>
    {
        private readonly IMenuServices _menuServices;
        public GetMenuByIdQueryHandler(IMenuServices menuServices)
        {
            _menuServices=menuServices;
        }
        public async Task<Result<GetMenuByIdResponse>> Handle(GetMenuByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var menuById = await _menuServices.GetMenuById(request.id);

                return Result<GetMenuByIdResponse>.Success(menuById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching subModules by Id", ex);
            }
        }
    }
    
}
