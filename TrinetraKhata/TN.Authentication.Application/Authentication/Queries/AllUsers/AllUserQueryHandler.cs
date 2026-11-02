using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Authentication.Application.Authentication.Queries.AllUsers
{
    public sealed class AllUserQueryHandler : IRequestHandler<AllUserQuery, Result<PagedResult<AllUserResponse>>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;

        public AllUserQueryHandler(IUserServices userServices, IMapper mapper)
        {
            _mapper = mapper;
            _userServices = userServices;
            
        }

        public async Task<Result<PagedResult<AllUserResponse>>> Handle(AllUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allUsers = await _userServices.GetAllUsers(request.PaginationRequest, cancellationToken);

                var alluserDisplay = _mapper.Map<PagedResult<AllUserResponse>>(allUsers.Data);

                return Result<PagedResult<AllUserResponse>>.Success(alluserDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while showing users");
            }
        }
    }
}
