using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TN.Authentication.Domain.Entities;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.AddOrganization;
using TN.Setup.Application.Setup.Command.UpdateOrganization;
using TN.Setup.Application.Setup.Queries.GetOrganizationByProvinceId;
using TN.Setup.Application.Setup.Queries.Organization;
using TN.Setup.Application.Setup.Queries.OrganizationById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using TN.Shared.Infrastructure.Repository;
using static System.Formats.Asn1.AsnWriter;

namespace TN.Setup.Infrastructure.ServiceImpl
{
    public class OrganizationServices : IOrganizationServices
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrganizationServices(IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _memoryCacheRepository = memoryCacheRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AddOrganizationResponse>> Add(AddOrganizationCommand addOrganizationCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    //await _memoryCacheRepository.RemoveAsync(CacheKeys.Organization);
                    string newId = Guid.NewGuid().ToString();

                    
                    var organizationData = new Organization
                    (
                        newId,
                        addOrganizationCommand.name,
                        addOrganizationCommand.address,
                        addOrganizationCommand.email,
                        addOrganizationCommand.phoneNumber,
                        addOrganizationCommand.mobileNumber,
                        addOrganizationCommand.logo,
                        addOrganizationCommand.provinceId

                    );

                    await _unitOfWork.BaseRepository<Organization>().AddAsync(organizationData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddOrganizationResponse>(organizationData);
                    return Result<AddOrganizationResponse>.Success(resultDTOs);


                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("an error occurred while adding organization", ex);
                }

            }
        }

        public async Task<Result<bool>> Delete(string Id, CancellationToken cancellationToken)
        {
            try
            {
                
                //await _memoryCacheRepository.RemoveAsync(CacheKeys.Organization);

                var organization = await _unitOfWork.BaseRepository<Organization>().GetByGuIdAsync(Id);
                if (organization is null)
                {
                    return Result<bool>.Failure("NotFound", "Organization Cannot be Found");
                }

                _unitOfWork.BaseRepository<Organization>().Delete(organization);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting organization having {Id}", ex);
            }

        }

        public async Task<Result<PagedResult<GetAllOrganizationResponse>>> GetAllOrganization(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                //var cachekeys = CacheKeys.Organization;
                //var cacheData = await _memoryCacheRepository.GetCacheKey<PagedResult<GetAllOrganizationResponse>>(cachekeys);
                //if(cacheData is not null) 
                //{
                //    return Result<PagedResult<GetAllOrganizationResponse>>.Success(cacheData);
                
                //}

                var organization = await _unitOfWork.BaseRepository<Organization>().GetAllAsyncWithPagination();
                var organizationPagedResult = await organization.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allOrganizationResponse = _mapper.Map<PagedResult<GetAllOrganizationResponse>>(organizationPagedResult.Data);

                //await _memoryCacheRepository.SetAsync(cachekeys, allOrganizationResponse, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions 
                //{
                //    AbsoluteExpiration=DateTimeOffset.Now.AddMinutes(30)
                //}, cancellationToken);
                return Result<PagedResult<GetAllOrganizationResponse>>.Success(allOrganizationResponse);

            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all organization", ex);
            }

        }

        public async Task<Result<GetOrganizationByIdQueryResponse>> GetOrganizationById(string organizationId, CancellationToken cancellationToken = default)
        {
            try
            {
                //var cachekeys = $"GetOrganizationById{CacheKeys.Organization}";
                //var cacheData = await _memoryCacheRepository.GetCacheKey<GetOrganizationByIdQueryResponse>(cachekeys);
                //if(cacheData is not null)
                //{
                //    return Result<GetOrganizationByIdQueryResponse>.Success(cacheData);
                //}

                var organization = await _unitOfWork.BaseRepository<Organization>().GetByGuIdAsync(organizationId);

                var districtResponse = _mapper.Map<GetOrganizationByIdQueryResponse>(organization);

                //await _memoryCacheRepository.SetAsync(cachekeys, districtResponse, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions {AbsoluteExpiration=DateTimeOffset.Now.AddMinutes(30) }, cancellationToken);
                return Result<GetOrganizationByIdQueryResponse>.Success(districtResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching organization  by using Id", ex);

            }
        }

        public async Task<Result<List<GetOrganizationByProvinceIdResponse>>> GetOrganizationByProvinceId(int provinceId, CancellationToken cancellationToken = default)
        {
            try
            {
                //var cachekeys = $"GetOrganizationByProvinceId{CacheKeys.Organization}";
                //var cacheData = await _memoryCacheRepository.GetCacheKey<List<GetOrganizationByProvinceIdResponse>>(cachekeys);
                //if (cacheData is not null)
                //{
                //    return Result<List<GetOrganizationByProvinceIdResponse>>.Success(cacheData);
                //}

                var district = await _unitOfWork.BaseRepository<Organization>().GetConditionalAsync(x => x.ProvinceId == provinceId);
                if (district is null)
                {
                    return Result<List<GetOrganizationByProvinceIdResponse>>.Failure("NotFound", "Organization Data are not found");
                }

                var organizationResponse = _mapper.Map<List<GetOrganizationByProvinceIdResponse>>(district);
                //await _memoryCacheRepository.SetAsync(cachekeys, organizationResponse, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30) }, cancellationToken);
                return Result<List<GetOrganizationByProvinceIdResponse>>.Success(organizationResponse);

            }
            catch  (Exception ex)
            {
                throw new Exception("An error occurred while fetching organization  by using province Id", ex);

            }
        }

        public async Task<Result<UpdateOrganizationResponse>> Update(string organizationId, UpdateOrganizationCommand updateOrganizationCommand)
        {

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    //await _memoryCacheRepository.RemoveAsync(CacheKeys.Organization);
                    if (organizationId == null)
                    {
                        return Result<UpdateOrganizationResponse>.Failure("NotFound", "Please provide valid organizationId");
                    }

                    var organizationToBeUpdated = await _unitOfWork.BaseRepository<Organization>().GetByGuIdAsync(organizationId);
                    if (organizationToBeUpdated is null)
                    {
                        return Result<UpdateOrganizationResponse>.Failure("NotFound", "LedgerGroup are not Found");
                    }

                    _mapper.Map(updateOrganizationCommand, organizationToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();
                    
                    var resultResponse = new UpdateOrganizationResponse
                        (
                            organizationToBeUpdated.Name,
                            organizationToBeUpdated.Address,
                            organizationToBeUpdated.Email,
                            organizationToBeUpdated.PhoneNumber,
                            organizationToBeUpdated.MobileNumber,
                            organizationToBeUpdated.Logo,
                            organizationToBeUpdated.ProvinceId

                        );

                    return Result<UpdateOrganizationResponse>.Success(resultResponse);
                    
                }
                catch (Exception ex)
                {
                   
                    throw new Exception($"An error occured while updating organization", ex);
                }
            }


        }
    }


}



