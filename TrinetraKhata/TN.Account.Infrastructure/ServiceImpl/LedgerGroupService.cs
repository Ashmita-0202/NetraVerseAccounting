using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TN.Account.Application.Account.Command.AddLedgerGroup;
using TN.Account.Application.Account.Command.UpdateLedgerGroup;
using TN.Account.Application.Account.Queries.FilterLedgerBySelectedLedgerGroup;
using TN.Account.Application.Account.Queries.FilterLedgerGroup;
using TN.Account.Application.Account.Queries.GetLedgerGroupByMasterId;
using TN.Account.Application.Account.Queries.LedgerGroup;
using TN.Account.Application.Account.Queries.LedgerGroupById;
using TN.Account.Application.ServiceInterface;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;


namespace TN.Account.Infrastructure.ServiceImpl
{
    public class LedgerGroupService : ILedgerGroupService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly FiscalContext _fiscalContext;

        public LedgerGroupService(IGetUserScopedData getUserScopedData,IDateConvertHelper dateConvertHelper,FiscalContext fiscalContext,ITokenService tokenService,IUnitOfWork unitOfWork,IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository= memoryCacheRepository;
            _getUserScopedData = getUserScopedData;
            _dateConvertHelper = dateConvertHelper;
            _fiscalContext= fiscalContext;


        }

        public async Task<Result<AddLedgerGroupResponse>> Add(AddLedgerGroupCommand addLedgerGroupCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string userId = _tokenService.GetUserId();
                    var fyId = _fiscalContext.CurrentFiscalYearId;
                    var ledgerGroupData = new LedgerGroup
                        (
                        
                            Guid.NewGuid().ToString(),
                            addLedgerGroupCommand.name,
                            addLedgerGroupCommand.isCustom,
                            addLedgerGroupCommand.masterId,
                            addLedgerGroupCommand.isPrimary,
                            _tokenService.CompanyId().FirstOrDefault() ?? "",
                            fyId,
                            userId,
                            DateTime.UtcNow,
                            "",
                            default,
                            false,
                            true
                        );

                    await _unitOfWork.BaseRepository<LedgerGroup>().AddAsync(ledgerGroupData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddLedgerGroupResponse>(ledgerGroupData);
                    return Result<AddLedgerGroupResponse>.Success(resultDTOs);



                }
                catch (Exception ex)

                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ledger Group", ex);

                };
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var ledgerGroup = await _unitOfWork.BaseRepository<LedgerGroup>().GetByGuIdAsync(id);
                ledgerGroup.IsActive = false;
                if (ledgerGroup is null)
                {
                    return Result<bool>.Failure("NotFound", "LedgerGroup Cannot be Found");
                }

                _unitOfWork.BaseRepository<LedgerGroup>().Update(ledgerGroup);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting ledgerGroup having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllLedgerGroupByQueryResponse>>> GetAllLedgerGroup(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var currentCompanyId = _tokenService.CompanyId().FirstOrDefault();


                var (ledgerGroup, scopedCompanyId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<LedgerGroup>();

                IQueryable<LedgerGroup> filterLedgerGroup;
                
                if(isSuperAdmin)
                {
                    filterLedgerGroup = ledgerGroup.Where(x => x.IsActive && x.FiscalId == fyId);
                }
                else
                {
                    filterLedgerGroup = ledgerGroup.Where(x =>x.IsActive &&
                 (x.FiscalId == fyId || x.FiscalId == "") &&
                 (x.CompanyId == currentCompanyId || x.CompanyId == "")
             );

                    if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(scopedCompanyId))
                    {
                        var companyIds = await _unitOfWork.BaseRepository<Company>()
                            .GetConditionalFilterType(
                                x => x.InstitutionId == institutionId,
                                query => query.Select(c => c.Id)
                            );

                        filterLedgerGroup = await _unitOfWork.BaseRepository<LedgerGroup>()
                            .FindBy(x =>x.IsActive && companyIds.Contains(x.CompanyId)
                            && (x.FiscalId == fyId || x.FiscalId == "")
                            );
                    }
                }


                var ledgerGroupPagedResult = await filterLedgerGroup.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allLedgerGroupResponse = _mapper.Map<PagedResult<GetAllLedgerGroupByQueryResponse>>(ledgerGroupPagedResult.Data);
             
                
                return Result<PagedResult<GetAllLedgerGroupByQueryResponse>>.Success(allLedgerGroupResponse);

            }
            catch (Exception ex)
           
            {
                throw new Exception("An error occurred while fetching all ledger group", ex);
            }
        }

        public async Task<Result<IEnumerable<FilterLedgerBySelectedLedgerGroupResponse>>> GetFilterLedger(CancellationToken cancellationToken)
        {
            try
            {
                List<string> groupNames = new() { "Bank Accounts", "Cash-in-hand", "Sundry Creditors" };

                var ledgerGroupIds = (await _unitOfWork.BaseRepository<Ledger>()
                .GetConditionalAsync(
                    predicate: x => groupNames.Contains(x.Name)
                ))
                .Select(x => x.SubLedgerGroupId)
                .Distinct()
                .ToList();


                var filterLedgerData = (await _unitOfWork.BaseRepository<Ledger>()
                .GetFilterAndOrderByAsync(x => ledgerGroupIds.Contains(x.SubLedgerGroupId)))
                .Select(x => new FilterLedgerBySelectedLedgerGroupResponse(x.Id, x.Name, x.SubLedgerGroupId));

                return Result<IEnumerable<FilterLedgerBySelectedLedgerGroupResponse>>.Success(filterLedgerData);




            }
            catch (Exception ex)
            {
                throw new Exception($"An error occured while filtering Ledger by selected LedgerGroupId",ex);
            }
        }



        public async Task<Result<PagedResult<GetFilterLedgerGroupQueryResponse>>> GetFilterLedgerGroup(PaginationRequest paginationRequest, FilterLedgerGroupDto filterLedgerGroupDto)
        {

            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;

                var (ledgerGroup, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<LedgerGroup>();


                var filterledgerGroup = isSuperAdmin
                    ? ledgerGroup
                    : ledgerGroup.Where(x =>x.IsActive && x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");

                DateTime startEnglishDate = filterLedgerGroupDto.startDate == default
                ? DateTime.MinValue
                    : await _dateConvertHelper.ConvertToEnglish(filterLedgerGroupDto.startDate);

                DateTime endEnglishDate = filterLedgerGroupDto.endDate == default
                ? DateTime.MaxValue
                    : await _dateConvertHelper.ConvertToEnglish(filterLedgerGroupDto.endDate);

                if (endEnglishDate != DateTime.MaxValue)
                {
                    endEnglishDate = endEnglishDate.Date.AddDays(1).AddTicks(-1);
                }

                var filteredResult = filterledgerGroup
                .Where(x =>
                (string.IsNullOrEmpty(filterLedgerGroupDto.name) ||
                         x.Name.ToLower().Contains(filterLedgerGroupDto.name.ToLower())) &&
                        (filterLedgerGroupDto.startDate == default || x.CreatedAt >= startEnglishDate) &&
                        (filterLedgerGroupDto.endDate == default || x.CreatedAt <= endEnglishDate)
                    )
                    .OrderBy(x => x.Name)
                    .ToList();

                var responseList = filteredResult.Select(sl => new GetFilterLedgerGroupQueryResponse(
                    sl.Id,
                    sl.Name,
                    sl.IsCustom,
                    sl.MasterId,
                    sl.IsPrimary,
                    DateTime.UtcNow,
                    sl.IsSeeded
                )).ToList();

                int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                int totalItems = responseList.Count;

                var pagedItems = responseList
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var pagedResult = new PagedResult<GetFilterLedgerGroupQueryResponse>
                {
                    Items = pagedItems,
                    TotalItems = totalItems,
                    PageIndex = pageIndex,
                    pageSize = pageSize
                };

                return Result<PagedResult<GetFilterLedgerGroupQueryResponse>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching ledgerGroup: {ex.Message}", ex);
            }
        }

        public async Task<Result<GetLedgerGroupByIdResponse>> GetLedgerGroupById(string ledgerGroupId, CancellationToken cancellationToken = default)
        {
            try {

                var ledgerGroup = await _unitOfWork.BaseRepository<LedgerGroup>().GetByGuIdAsync(ledgerGroupId);

                var ledgerGroupResponse = _mapper.Map<GetLedgerGroupByIdResponse>(ledgerGroup);

                return Result<GetLedgerGroupByIdResponse>.Success(ledgerGroupResponse);

            }
            catch (Exception ex) 
           
            { 
                throw new Exception("An error occurred while fetching Ledger Group by using Id", ex);
            
            }
        }

        public async Task<Result<List<GetLedgerGroupByMasterIdResponse>>> GetLedgerGroupByMasterId(string masterId, CancellationToken cancellationToken = default)
        {
            try
            {
                var ledgerGroup = await _unitOfWork.BaseRepository<LedgerGroup>().GetConditionalAsync(x => x.MasterId == masterId);
                if (ledgerGroup is null)
                {
                    return Result<List<GetLedgerGroupByMasterIdResponse>>.Failure("NotFound", "LedgerGroup Data are not found");
                }

                var ledgerGroupResponse = _mapper.Map<List<GetLedgerGroupByMasterIdResponse>>(ledgerGroup);

                return Result<List<GetLedgerGroupByMasterIdResponse>>.Success(ledgerGroupResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Ledger Group by using masterId",ex);
            }
        }

        
        public async Task<Result<UpdateLedgerGroupResponse>> Update(string ledgerGroupId, UpdateLedgerGroupCommand updateLedgerGroupCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (ledgerGroupId == null)
                    {
                        return Result<UpdateLedgerGroupResponse>.Failure("NotFound", "Please provide valid ledgerGroupId");
                    }

                    var ledgerGroupToBeUpdated = await _unitOfWork.BaseRepository<LedgerGroup>().GetByGuIdAsync(ledgerGroupId);
                    if (ledgerGroupToBeUpdated is null)
                    {
                        return Result<UpdateLedgerGroupResponse>.Failure("NotFound", "LedgerGroup are not Found");
                    }

                    _mapper.Map(updateLedgerGroupCommand, ledgerGroupToBeUpdated);

                    ledgerGroupToBeUpdated.ModifiedBy = "";
                    ledgerGroupToBeUpdated.ModifiedAt = DateTime.UtcNow;


                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateLedgerGroupResponse
                        (
                        ledgerGroupToBeUpdated.Name,
                        ledgerGroupToBeUpdated.IsCustom,
                        ledgerGroupToBeUpdated.MasterId,
                        ledgerGroupToBeUpdated.IsPrimary
                        );

                    return Result<UpdateLedgerGroupResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occured while Ledger Group updated", ex);
                }
            }
        }
    }
}
