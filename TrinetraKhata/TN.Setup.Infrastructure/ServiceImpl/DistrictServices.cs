using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Authentication.Domain.Entities;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.AddCompany;
using TN.Setup.Application.Setup.Queries.Company;
using TN.Setup.Application.Setup.Queries.CompanyById;
using TN.Setup.Application.Setup.Queries.CompanyByInstitutionId;
using TN.Setup.Application.Setup.Queries.District;
using TN.Setup.Application.Setup.Queries.DistrictById;
using TN.Setup.Application.Setup.Queries.GetDistrictByProvinceId;
using TN.Setup.Application.Setup.Queries.Province;
using TN.Setup.Application.Setup.Queries.ProvinceById;
using TN.Setup.Domain.Entities;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using TN.Shared.Infrastructure.Repository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TN.Setup.Infrastructure.ServiceImpl
{
    public class DistrictServices : IDistrictServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMemoryCacheRepository _memoryCacheRepository;
        private readonly IMapper _mapper;

        public DistrictServices(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCacheRepository memoryCacheRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;

        }
        public async Task<Result<PagedResult<GetAllDistrictResponse>>> GetAllDistrict(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var cacheKeys = CacheKeys.District;

                var cacheData = await _memoryCacheRepository.GetCacheKey<PagedResult<GetAllDistrictResponse>>(cacheKeys);
                if (cacheData is not null)
                {
                    return Result<PagedResult<GetAllDistrictResponse>>.Success(cacheData);
                }

                var district = await _unitOfWork.BaseRepository<District>().GetAllAsyncWithPagination();
                var districtPagedResult = await district.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);
                var allDistrictResponse = _mapper.Map<PagedResult<GetAllDistrictResponse>>(districtPagedResult.Data);
                await _memoryCacheRepository.SetAsync(cacheKeys, allDistrictResponse, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);
                return Result<PagedResult<GetAllDistrictResponse>>.Success(allDistrictResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all District", ex);
            }
        }

        public async Task<Result<GetDistrictByIdResponse>> GetDistrictById(int districtId, CancellationToken cancellationToken = default)
        {
            try
            {
                var cacheKey = $"GetDistrictById{districtId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<GetDistrictByIdResponse>(cacheKey);
                if (cacheData is not null)
                {
                    return Result<GetDistrictByIdResponse>.Success(cacheData);
                }
                var district = await _unitOfWork.BaseRepository<District>().GetById(districtId);

                var districtResponse = _mapper.Map<GetDistrictByIdResponse>(district);

                await _memoryCacheRepository.SetAsync(cacheKey, districtResponse, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);
                return Result<GetDistrictByIdResponse>.Success(districtResponse);
               

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching district  by using Id", ex);
            }
        }

        public async Task<Result<List<GetDistrictByProvinceIdResponse>>> GetDistrictByProvinceId(int provinceId, CancellationToken cancellationToken = default)
        {
            try
            {                             
                var district = await _unitOfWork.BaseRepository<District>().GetConditionalAsync(x=>x.ProvinceId == provinceId);
                if( district is null )
                {
                    return Result<List<GetDistrictByProvinceIdResponse>>.Failure("NotFound", "District Data are not found");
                }
                var districtResponse = _mapper.Map<List<GetDistrictByProvinceIdResponse>>( district);
               
                return Result<List<GetDistrictByProvinceIdResponse>>.Success(districtResponse);
            }catch(Exception ex)
            {
                throw new Exception("An error occurred while fetching district using provinceId", ex);
            }
        }
    }
}
