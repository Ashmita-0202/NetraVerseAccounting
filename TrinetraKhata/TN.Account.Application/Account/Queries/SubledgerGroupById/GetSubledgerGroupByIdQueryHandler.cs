using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.SubledgerGroupById
{
    public class GetSubledgerGroupByIdQueryHandler:IRequestHandler<GetSubledgerGroupByIdQuery, Result<GetSubledgerGroupByIdResponse>>
    {
        private readonly ISubledgerGroupService _service;
        private readonly IMapper _mapper;

        public GetSubledgerGroupByIdQueryHandler(ISubledgerGroupService service,IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<Result<GetSubledgerGroupByIdResponse>> Handle(GetSubledgerGroupByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subledgerGroupById = await _service.GetById(request.id);
                return Result<GetSubledgerGroupByIdResponse>.Success(subledgerGroupById.Data);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching subledger group by Id", ex);

            }  
        }
    }
}
