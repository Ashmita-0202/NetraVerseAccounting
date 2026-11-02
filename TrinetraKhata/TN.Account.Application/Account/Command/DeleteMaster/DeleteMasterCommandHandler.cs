using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.DeleteMaster
{
public class DeleteMasterCommandHandler:IRequestHandler<DeleteMasterCommand,Result<bool>>
    {
        private readonly IMasterService _masterService;
        private readonly IMapper _mapper;

        public DeleteMasterCommandHandler(IMasterService masterService,IMapper mapper) 
        { 
          _masterService=masterService;
            _mapper=mapper;
        }

        public async Task<Result<bool>> Handle(DeleteMasterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteMaster = await _masterService.Delete(request.Id, cancellationToken);
                if (deleteMaster is null)
                {
                    return Result<bool>.Failure("NotFound", "Master not Found");
                }
                return Result<bool>.Success(true);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.Id}", ex);
            }
        }
    }
}
