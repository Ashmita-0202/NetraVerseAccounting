using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Authentication.Domain.Entities;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.Province;
using TN.Setup.Application.Setup.Queries.ProvinceById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;

namespace TN.Setup.Infrastructure.ServiceImpl
{
    public class ProvinceServices : IProvinceServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public ProvinceServices(IUnitOfWork unitOfWork,IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _mapper = mapper;
            _memoryCacheRepository= memoryCacheRepository;
            _unitOfWork = unitOfWork;
            
        }
        public async Task<Result<PagedResult<GetAllProvinceResponse>>> GetAllProvince(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var cachekeys = CacheKeys.Province;
                var cacheData = await _memoryCacheRepository.GetCacheKey<PagedResult<GetAllProvinceResponse>>(cachekeys);
                if (cacheData is not null) 
                {
                    return Result<PagedResult<GetAllProvinceResponse>>.Success(cacheData);
                
                }

                var province = await _unitOfWork.BaseRepository<Province>().GetAllAsyncWithPagination();
                var provincePagedResult = await province.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allProvinceResponse = _mapper.Map<PagedResult<GetAllProvinceResponse>> (provincePagedResult.Data);

                await _memoryCacheRepository.SetAsync(cachekeys, allProvinceResponse, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30) }, cancellationToken);
                return Result<PagedResult<GetAllProvinceResponse>>.Success(allProvinceResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Province", ex);
            }
        }

        public async Task<Result<GetProvinceByIdResponse>> GetProvinceById(int provinceId, CancellationToken cancellationToken = default)
        {
            try
            {
                var cachekeys = $"GetProvinceById{CacheKeys.Province}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<GetProvinceByIdResponse>(cachekeys);
                if (cacheData is not null) 
                {
                  return Result<GetProvinceByIdResponse>.Success(cacheData);
                  
                }


                var province  = await _unitOfWork.BaseRepository<Province>().GetById(provinceId);

                var provinceResponse = _mapper.Map<GetProvinceByIdResponse>(province);
               
                await _memoryCacheRepository.SetAsync(cachekeys,provinceResponse,new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions { AbsoluteExpiration=DateTimeOffset.Now.AddMinutes(30)}, cancellationToken);
                
                return Result<GetProvinceByIdResponse>.Success(provinceResponse);

            }catch(Exception ex)
            {
                throw new Exception("An error occurred while fetching province by using Id", ex);
            }
        }
    }
}
