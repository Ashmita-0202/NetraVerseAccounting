using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Account.Application.Account.Command.AddSubledgerGroup;
using TN.Account.Application.Account.Command.UpdateSubledgerGroup;
using TN.Account.Application.Account.Queries.FilterParties;
using TN.Account.Application.Account.Queries.FilterSubledgerGroupByDate;
using TN.Account.Application.Account.Queries.SubledgerGroup;
using TN.Account.Application.Account.Queries.SubledgerGroupById;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;


namespace TN.Account.Infrastructure.ServiceImpl
{
    public class SubledgerGroupService : ISubledgerGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly FiscalContext _fiscalContext;

        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly ITokenService _tokenService;

        public SubledgerGroupService(IUnitOfWork unitOfWork,ITokenService tokenService, IGetUserScopedData getUserScopedData, IMapper mapper, FiscalContext fiscalContext,IDateConvertHelper dateConvertHelper)

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fiscalContext = fiscalContext;

            _dateConvertHelper = dateConvertHelper;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;


        }
        public async Task<Result<AddSubledgerGroupResponse>> Add(AddSubledgerGroupCommand command)
        {

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var companyId = _tokenService.CompanyId().FirstOrDefault();
                    var userId = _tokenService.GetUserId();
                    string newId = Guid.NewGuid().ToString();
                    //var FyId = _fiscalContext.CurrentFiscalYearId;


                    var subledgerGroupData = new SubLedgerGroup(
                         newId,
                         command.name,
                         command.ledgerGroupId,
                         companyId,
                         userId,
                         DateTime.Now,
                         "",
                         default,
                         false,
                         true
                    );

                    await _unitOfWork.BaseRepository<SubLedgerGroup>().AddAsync(subledgerGroupData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddSubledgerGroupResponse>(subledgerGroupData);
                    return Result<AddSubledgerGroupResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding subledgerGroup ", ex);

                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {

                var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>().GetByGuIdAsync(id);
                subledgerGroup.IsActive = false;
                if (subledgerGroup is null)
                {
                    return Result<bool>.Failure("NotFound", "subledgerGroup Cannot be Found");
                }

                _unitOfWork.BaseRepository<SubLedgerGroup>().Update(subledgerGroup);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting subledgerGroup having {id}", ex);
            }

        }

        public async Task<Result<PagedResult<GetAllSubledgerGroupQueryResposne>>> GetAll(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var (subLedgerGroups, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                 await _getUserScopedData.GetUserScopedData<SubLedgerGroup>();
               
                var queryable = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                    .FindBy(x => (x.IsActive && x.IsSeeded.HasValue && x.IsSeeded.Value) ||
                                ((x.IsSeeded.HasValue && !x.IsSeeded.Value) && x.CompanyId == currentCompanyId));

               
                var finalQuery = queryable.AsNoTracking();

                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);

                var mappedItems = _mapper.Map<List<GetAllSubledgerGroupQueryResposne>>(pagedResult.Data.Items);

                var response = new PagedResult<GetAllSubledgerGroupQueryResposne>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<GetAllSubledgerGroupQueryResposne>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all subledger groups", ex);
            }

        }

        public async Task<Result<GetSubledgerGroupByIdResponse>> GetById(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var subLedgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>().GetByGuIdAsync(id);

                var subLedgerGroupResponse = _mapper.Map<GetSubledgerGroupByIdResponse>(subLedgerGroup);

                return Result<GetSubledgerGroupByIdResponse>.Success(subLedgerGroupResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching SubledgerGroup by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<GetFilterSubledgerGroupQueryResponse>>> GetFilterSubLedgerGroup(PaginationRequest paginationRequest, FilterSubledgerGroupDto filterSubledgerGroupDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;

                var (subledgerGroup, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<SubLedgerGroup>();

                
                var filterSubledgerGroup = isSuperAdmin
                    ? subledgerGroup
                    : subledgerGroup.Where(x =>x.IsActive && x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");

                DateTime startEnglishDate = filterSubledgerGroupDto.startDate == default
                    ? DateTime.MinValue
                    : await _dateConvertHelper.ConvertToEnglish(filterSubledgerGroupDto.startDate);

                DateTime endEnglishDate = filterSubledgerGroupDto.endDate == default
                    ? DateTime.MaxValue
                    : await _dateConvertHelper.ConvertToEnglish(filterSubledgerGroupDto.endDate);

                if (endEnglishDate != DateTime.MaxValue)
                {
                    endEnglishDate = endEnglishDate.Date.AddDays(1).AddTicks(-1);
                }

                var filteredResult = filterSubledgerGroup
                    .Where(x =>
                        (string.IsNullOrEmpty(filterSubledgerGroupDto.name) ||
                         x.Name.ToLower().Contains(filterSubledgerGroupDto.name.ToLower())) &&
                        (filterSubledgerGroupDto.startDate == default || x.CreatedAt >= startEnglishDate) &&
                        (filterSubledgerGroupDto.endDate == default || x.CreatedAt <= endEnglishDate)
                    )
                    .OrderBy(x => x.Name)
                    .ToList();

                var responseList = filteredResult.Select(sl => new GetFilterSubledgerGroupQueryResponse(
                    sl.Id,
                    sl.Name,
                    sl.LedgerGroupId,
                    sl.IsSeeded

                )).ToList();

                PagedResult<GetFilterSubledgerGroupQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetFilterSubledgerGroupQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetFilterSubledgerGroupQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }

                return Result<PagedResult<GetFilterSubledgerGroupQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching SubledgerGroup: {ex.Message}", ex);
            }
        }

        public async Task<Result<UpdateSubledgerGroupResponse>> Update(string id, UpdateSubledgerGroupCommand command)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateSubledgerGroupResponse>.Failure("NotFound", "Please provide valid subledgerGroupId");
                    }

                    var subledgerGroupToBeUpdated = await _unitOfWork.BaseRepository<SubLedgerGroup>().GetByGuIdAsync(id);
                    if (subledgerGroupToBeUpdated is null)
                    {
                        return Result<UpdateSubledgerGroupResponse>.Failure("NotFound", "SubledgerGroup are not Found");
                    }
                    subledgerGroupToBeUpdated.ModifiedBy = _tokenService.GetUserId();
                    subledgerGroupToBeUpdated.ModifiedAt = DateTime.Now;
                    _mapper.Map(command, subledgerGroupToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateSubledgerGroupResponse
                        (

                            subledgerGroupToBeUpdated.Name,
                            subledgerGroupToBeUpdated.LedgerGroupId

                        );

                    return Result<UpdateSubledgerGroupResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("");
                }
            }
        }

    }
}