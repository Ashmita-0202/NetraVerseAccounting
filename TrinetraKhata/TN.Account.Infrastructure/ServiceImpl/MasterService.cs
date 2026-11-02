using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TN.Account.Application.Account.Command.UpdateMaster;
using TN.Account.Application.Account.Queries.GetMasterById;
using TN.Account.Application.Account.Queries.master;
using TN.Account.Application.ServiceInterface;
using TN.Account.Domain.Entities;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;

namespace TN.Account.Infrastructure.ServiceImpl
{
    public class MasterService : IMasterService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public MasterService(IUnitOfWork unitOfWork, IMapper mapper,IMemoryCacheRepository memoryCacheRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository= memoryCacheRepository;
        }

        public async Task<Result<bool>> Delete(string Id, CancellationToken cancellationToken)
        {
            try
            {
                await _memoryCacheRepository.RemoveAsync(CacheKeys.Master);
                var master = await _unitOfWork.BaseRepository<Master>().GetByGuIdAsync(Id);
                master.IsActive = false;
                if (master is null)
                {
                    return Result<bool>.Failure("NotFound", "LedgerGroup Cannot be Found");
                }

                _unitOfWork.BaseRepository<Master>().Update(master);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting master having {Id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllMasterByQueryResponse>>> GetAllMaster(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {

            try
            {
                //var cachekeys = CacheKeys.Master;
                //var cacheData= await _memoryCacheRepository.GetCacheKey<PagedResult<GetAllMasterByQueryResponse>>(cachekeys);
                //if (cacheData is not null) 
                //{
                //    return Result<PagedResult<GetAllMasterByQueryResponse>>.Success(cacheData);
                
                //}

                var master = await _unitOfWork.BaseRepository<Master>().GetAllAsyncWithPagination();
                var masterPagedResult = await master.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allMasterResponse = _mapper.Map<PagedResult<GetAllMasterByQueryResponse>>(masterPagedResult.Data);

                //await _memoryCacheRepository.SetAsync(cachekeys, allMasterResponse,new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions { AbsoluteExpiration=DateTimeOffset.Now.AddMinutes(30)},cancellationToken);
                return Result<PagedResult<GetAllMasterByQueryResponse>>.Success(allMasterResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Master", ex);
            }


        }

        public async Task<Result<GetMasterByIdQueryResponse>> GetMasterById(string masterId, CancellationToken cancellationToken = default)
        {
            try
            {
                //var cachekeys=$"GetAllMaster{CacheKeys.Master}";
                //var cacheData=await _memoryCacheRepository.GetCacheKey<GetMasterByIdQueryResponse>(cachekeys);
                //if(cacheData is not null)
                //{
                //    return Result<GetMasterByIdQueryResponse>.Success(cacheData);

                //}
                var master = await _unitOfWork.BaseRepository<Master>().GetByGuIdAsync(masterId);

                var masterResponse = _mapper.Map<GetMasterByIdQueryResponse>(master);

                //await _memoryCacheRepository.SetAsync(cachekeys,masterResponse,new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions { AbsoluteExpiration= DateTimeOffset.Now.AddMinutes(30)}, cancellationToken);
                return Result<GetMasterByIdQueryResponse>.Success(masterResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching master  by using Id", ex);
            }
        }

        public async Task<Result<UpdateMasterResponse>> Update(string id, UpdateMasterCommand updateMasterCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateMasterResponse>.Failure("NotFound", "Please provide valid ledgerGroupId");
                    }

                    var masterToBeUpdated = await _unitOfWork.BaseRepository<Master>().GetByGuIdAsync(id);
                    if (masterToBeUpdated is null)
                    {
                        return Result<UpdateMasterResponse>.Failure("NotFound", "Master are not Found");
                    }

                    _mapper.Map(updateMasterCommand, masterToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateMasterResponse
                        (
                               
                            masterToBeUpdated.Name

                        );

                    return Result<UpdateMasterResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("");
                }
            }

        }
    }

}
