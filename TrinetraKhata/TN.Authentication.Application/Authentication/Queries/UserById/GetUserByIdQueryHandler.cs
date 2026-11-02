using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Queries.UserById
{
    public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<GetUserByIdResponse>>
    {
        private readonly IUserServices _userServices;

        public GetUserByIdQueryHandler(IUserServices userServices)
        {
            _userServices = userServices;
            
        }
        public async Task<Result<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userById = await _userServices.GetUserById(request.userId, cancellationToken);
                return Result<GetUserByIdResponse>.Success(userById.Data);

            }catch (Exception ex)
            {
                throw new Exception($"An error occured while fetching Users By {request.userId}");
            }
        }
    }
}
