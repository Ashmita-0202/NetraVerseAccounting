using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.GetMasterById
{
    public sealed class GetMasterByIdQueryHandler : IRequestHandler<GetMasterByIdQuery, Result<GetMasterByIdQueryResponse>>
    {
        private readonly IMasterService _service;
        private readonly IMapper _mapper;

        public GetMasterByIdQueryHandler(IMasterService masterService, IMapper mapper)
        {
            _service= masterService;
            _mapper= mapper;
        }

        public async Task<Result<GetMasterByIdQueryResponse>> Handle(GetMasterByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var masterById = await _service.GetMasterById(request.Id);

                return Result<GetMasterByIdQueryResponse>.Success(masterById.Data);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching Master by Id", ex);

            }
        }
    }
}
