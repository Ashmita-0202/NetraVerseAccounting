using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.DeleteSubledgerGroup
{
    public class  DeleteSubledgerGroupCommandHandler:IRequestHandler<DeleteSubledgerGroupCommand, Result<bool>>
    {
        private readonly ISubledgerGroupService _service;
        private readonly IMapper _mapper;

        public DeleteSubledgerGroupCommandHandler(ISubledgerGroupService service,IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<Result<bool>> Handle(DeleteSubledgerGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteSubledgerGroup = await _service.Delete(request.id, cancellationToken);
                if (deleteSubledgerGroup is null)
                {
                    return Result<bool>.Failure("NotFound", "SubledgerGroup not Found");
                }
                return Result<bool>.Success(true);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.id}", ex);
            }
        }
    }
}
