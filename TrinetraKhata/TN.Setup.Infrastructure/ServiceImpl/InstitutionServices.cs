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
using TN.Authentication.Domain.Static.Roles;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.AddInstitution;
using TN.Setup.Application.Setup.Command.UpdateInstitution;
using TN.Setup.Application.Setup.Queries.Institution;
using TN.Setup.Application.Setup.Queries.InstitutionById;
using TN.Setup.Application.Setup.Queries.InstitutionByOrganizationId;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace TN.Setup.Infrastructure.ServiceImpl
{
    public class InstitutionServices : IInstitutionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;

        public InstitutionServices(IGetUserScopedData getUserScopedData,IUnitOfWork unitOfWork,ITokenService tokenService, IMemoryCacheRepository memoryCacheRepository, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _memoryCacheRepository= memoryCacheRepository;
            _mapper = mapper;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _getUserScopedData = getUserScopedData;
        }
        public async Task<Result<AddInstitutionResponse>> Add(AddInstitutionCommand addInstitutionCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string userId = _tokenService.GetUserId();
                    string newId = Guid.NewGuid().ToString();
                    var institutionData = new Institution
                    (
                          newId,
                          addInstitutionCommand.name,
                          addInstitutionCommand.address,
                          addInstitutionCommand.email,
                          addInstitutionCommand.shortName,
                          addInstitutionCommand.contactNumber,
                          addInstitutionCommand.contactPerson,
                          addInstitutionCommand.pan,
                          addInstitutionCommand.imageUrl,
                          DateTime.UtcNow,
                          userId,
                          default,
                          "",
                          addInstitutionCommand.isDeleted,
                          addInstitutionCommand.organizationId

                     );

                    await _unitOfWork.BaseRepository<Institution>().AddAsync(institutionData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddInstitutionResponse>(institutionData);
                    return Result<AddInstitutionResponse>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Instituion", ex);

                }
            }
        }

        public async Task<Result<bool>> Delete(string Id, CancellationToken cancellationToken)
        {
            try
            {
                
                //await _memoryCacheRepository.RemoveAsync(CacheKeys.Institution);

                var institution = await _unitOfWork.BaseRepository<Institution>().GetByGuIdAsync(Id);
                if (institution is null)
                {
                    return Result<bool>.Failure("NotFound", "Module Cannot be Found");
                }

                _unitOfWork.BaseRepository<Institution>().Delete(institution);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Institution having {Id}", ex);
            }

        }

        public async Task<Result<PagedResult<GetAllInstitutionResponse>>> GetAllInstitution(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var checkInstitution = await _unitOfWork.BaseRepository<Institution>()
                   .FindBy(x => x.Id == _tokenService.InstitutionId()
                   );
                IQueryable<Institution> allInstitution = await _unitOfWork.BaseRepository<Institution>().GetAllAsyncWithPagination();
                var userRoles = _tokenService.GetRole();
                var isSuperAdmin = userRoles == Role.SuperAdmin;


                var filterInstitution = isSuperAdmin ? allInstitution : checkInstitution;


                var institutionPagedResult = await filterInstitution.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);
                var allInstitutionResponse = _mapper.Map<PagedResult<GetAllInstitutionResponse>>(institutionPagedResult.Data);

                return Result<PagedResult<GetAllInstitutionResponse>>.Success(allInstitutionResponse);
            }
            catch (Exception ex) 
            {
                throw new Exception("An error occurred while fetching the Institution",ex);
            
            }
        }

        public async Task<Result<GetInstitutionByIdResponse>> GetInstitutionById(string institutionId, CancellationToken cancellationToken = default)
        {
            try 
            {
                var institution = await _unitOfWork.BaseRepository<Institution>().GetByGuIdAsync(institutionId);

                var institutionResponse = _mapper.Map<GetInstitutionByIdResponse>(institution);
                return Result<GetInstitutionByIdResponse>.Success(institutionResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("an error occurred while fetching Instution by Id", ex);
            }
        }

        public async Task<Result<List<GetInstitutionByOrganizationIdResponse>>> GetInstitutionByOrganizationId(string organizationId, CancellationToken cancellationToken = default)
        {
            try 
            {
                //var cacheKey = $"GetInstitutionByOrganizationId{CacheKeys.Institution}";
                //var cacheData = await _memoryCacheRepository.GetCacheKey<List<GetInstitutionByOrganizationIdResponse>>(cacheKey);
                //if (cacheData is not null)
                //{
                //    return Result<List<GetInstitutionByOrganizationIdResponse>>.Success(cacheData);
                //}

                var instituion = await _unitOfWork.BaseRepository<Institution>().GetConditionalAsync(x => x.OrganizationId == organizationId);
                if (instituion is null)
                {
                    return Result<List<GetInstitutionByOrganizationIdResponse>>.Failure("NotFound", "Institution Data are not found");
                }

                var institutionResponse = _mapper.Map<List<GetInstitutionByOrganizationIdResponse>>(instituion);

                //await _memoryCacheRepository.SetAsync(cacheKey, institutionResponse, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                //{
                //    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                //}, cancellationToken);
                return Result<List<GetInstitutionByOrganizationIdResponse>>.Success(institutionResponse);
            }
            catch (Exception ex)
            
            {
                throw new Exception("an error occurred while fetching Institution by Organization Id", ex);
            }
            
        }

        public async Task<Result<UpdateInstitutionResponse>> Update(string institutionId, UpdateInstitutionCommand updateInstitutionCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var userId = _tokenService.GetUserId();
                    if (institutionId == null)
                    {
                        return Result<UpdateInstitutionResponse>.Failure("NotFound", "Please provide valid institutionId");
                    }

                    var institutionToBeUpdated = await _unitOfWork.BaseRepository<Institution>().GetByGuIdAsync(institutionId);
                    if (institutionToBeUpdated is null)
                    {
                        return Result<UpdateInstitutionResponse>.Failure("NotFound", "LedgerGroup are not Found");
                    }

                    institutionToBeUpdated.ModifiedBy = userId;
                    institutionToBeUpdated.ModifiedDate = DateTime.UtcNow;

                    _mapper.Map(updateInstitutionCommand, institutionToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateInstitutionResponse
                        (
                            institutionId,
                            institutionToBeUpdated.Name,
                            institutionToBeUpdated.Address,
                            institutionToBeUpdated.Email,
                            institutionToBeUpdated.ShortName,
                            institutionToBeUpdated.ContactNumber,
                            institutionToBeUpdated.ContactPerson,
                            institutionToBeUpdated.PAN,
                            institutionToBeUpdated.ImageUrl,
                            institutionToBeUpdated.CreatedDate,
                            institutionToBeUpdated.CreatedBy,
                            institutionToBeUpdated.ModifiedDate,
                            institutionToBeUpdated.ModifiedBy,
                            institutionToBeUpdated.IsDeleted,
                            institutionToBeUpdated.OrganizationId
                        );


                    return Result<UpdateInstitutionResponse>.Success(resultResponse);


                }
                catch (Exception ex)
                {

                    throw new Exception("An error occur while updating the institution", ex);
                }

            }
        }
    }
}


