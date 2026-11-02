using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Queries.FilterUserByDate
{
   public class FilterUserByDateQueryHandler:IRequestHandler<FilterUserByDateQuery,Result<IEnumerable<FilterUserByDateQueryResponse>>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;

        public FilterUserByDateQueryHandler(IUserServices userServices,IMapper mapper)
        {
            _userServices=userServices;
            _mapper=mapper;
        }

        public async Task<Result<IEnumerable<FilterUserByDateQueryResponse>>> Handle(FilterUserByDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterUser = await _userServices.GetUserFilter(request.FilterUserDTOs, cancellationToken);

                if (!filterUser.IsSuccess || filterUser.Data == null)
                {
                    return Result<IEnumerable<FilterUserByDateQueryResponse>>.Failure(filterUser.Message);
                }

                var filterUserResult = _mapper.Map<List<FilterUserByDateQueryResponse>>(filterUser.Data);

                return Result<IEnumerable<FilterUserByDateQueryResponse>>.Success(filterUserResult);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<FilterUserByDateQueryResponse>>.Failure(
                    $"An error occurred while fetching User  by date: {ex.Message}");
            }
        }
    }
}
