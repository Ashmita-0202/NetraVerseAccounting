using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
using TN.Inventory.Application.Inventory.Command.AddStockAdjustment;
using TN.Inventory.Application.Inventory.Command.UpdateStockAdjustment;
using TN.Inventory.Application.Inventory.Queries.FilterInventoryByDate;
using TN.Inventory.Application.Inventory.Queries.FilterStockAdjustment;
using TN.Inventory.Application.Inventory.Queries.GetAllInventory;
using TN.Inventory.Application.Inventory.Queries.GetAllInventoryLogs;
using TN.Inventory.Application.Inventory.Queries.GetAllStockAdjustment;
using TN.Inventory.Application.Inventory.Queries.GetRemainingQtyByItemId;
using TN.Inventory.Application.Inventory.Queries.InventoriesLogsById;
using TN.Inventory.Application.Inventory.Queries.InventoryByItem;
using TN.Inventory.Application.ServiceInterface;
using TN.Inventory.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Inventory;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IActivityProcessServices;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using static TN.Inventory.Domain.Entities.Inventories;

namespace TN.Inventory.Infrastructure.ServiceImpl
{
    public class InventoriesServices : IInventoriesServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly FiscalContext _fiscalContext;
        private readonly IStockAlertActivityProcessServices _stockAlertActivityProcessServices;


        public InventoriesServices(IGetUserScopedData getUserScopedData,IStockAlertActivityProcessServices stockAlertActivityProcessServices, FiscalContext fiscalContext, IDateConvertHelper dateConvertHelper, IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _dateConvertHelper = dateConvertHelper;
            _mapper = mapper;
            _tokenService = tokenService;
            _getUserScopedData = getUserScopedData;
            _fiscalContext = fiscalContext;
            _stockAlertActivityProcessServices = stockAlertActivityProcessServices;

        }

        public async Task<Result<AddStockAdjustmentResponse>> AddStockAdjustment(AddStockAdjustmentCommand command)
        {
            try
            {

                string userId = _tokenService.GetUserId();
                string companyId = _tokenService.CompanyId().FirstOrDefault() ?? "";


                var itemsQuery = await _unitOfWork.BaseRepository<Items>().FindBy(x => x.Id == command.itemId);
                var item = await itemsQuery.FirstOrDefaultAsync();

                if (item == null)
                    return Result<AddStockAdjustmentResponse>.Failure("Item not found");

                var stockAdjustment = new StockAdjustment(
                    Guid.NewGuid().ToString(),
                    command.itemId,
                    command.quantityChanged,
                    command.reason,
                    DateTime.UtcNow,
                    userId,
                    companyId
                );

                decimal costPrice = 0;
                if (!decimal.TryParse(item.CostPrice, out costPrice))
                    throw new InvalidOperationException("Invalid cost price");

                decimal qtyChanged = Convert.ToDecimal(command.quantityChanged);
                var totalAmount = Math.Abs(costPrice) * Math.Abs(qtyChanged);



                var inventoryRepo = _unitOfWork.BaseRepository<Inventories>();

                // Await FindBy first to get IQueryable<Inventories>
                var inventoriesQuery = await inventoryRepo.FindBy(x => x.ItemId == command.itemId && x.companyId == companyId);
                var inventories = await inventoriesQuery.OrderByDescending(x => x.EntryDate).ToListAsync();

                if (command.quantityChanged > 0)
                {

                    var latestInventory = inventories.FirstOrDefault();
                    if (latestInventory == null)
                    {
                        return Result<AddStockAdjustmentResponse>.Failure("No inventory record found to add quantity.");
                    }
                    latestInventory.QuantityIn += (decimal)command.quantityChanged;
                    _unitOfWork.BaseRepository<Inventories>().Update(latestInventory);
                }
                else if (command.quantityChanged < 0)
                {
                    decimal qtyToDeduct = Math.Abs((decimal)command.quantityChanged);
                    var fifoInventories = inventories.OrderBy(x => x.EntryDate).ToList();

                    foreach (var inventory in fifoInventories)
                    {
                        var availableQty = inventory.QuantityIn - inventory.QuantityOut;
                        if (availableQty <= 0) continue;

                        if (availableQty >= qtyToDeduct)
                        {
                            inventory.QuantityOut += qtyToDeduct;
                            qtyToDeduct = 0;
                            break;
                        }
                        else
                        {
                            inventory.QuantityOut += availableQty;
                            qtyToDeduct -= availableQty;
                        }

                        _unitOfWork.BaseRepository<Inventories>().Update(inventory);

                        if (qtyToDeduct == 0)
                            break;
                    }

   

                    if (qtyToDeduct > 0)
                        return Result<AddStockAdjustmentResponse>.Failure("Not enough stock in inventory to deduct.");
                }



                await _unitOfWork.BaseRepository<StockAdjustment>().AddAsync(stockAdjustment);
                item.OpeningStockQuantity ??= 0;
                //item.OpeningStockQuantity += (decimal?)command.quantityChanged;


                #region AddJournalEntries

                string newJournalId = Guid.NewGuid().ToString();
                var journalDetails = new List<JournalEntryDetails>();
                var FyId = _fiscalContext.CurrentFiscalYearId;

                #region Add Journal

                switch (command.reason)
                {
                    case StockAdjustment.ReasonType.DamagedLossTheft:
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            LedgerConstants.StockLossLedger,
                            totalAmount,
                            0,
                            DateTime.Now,
                            companyId,
                            FyId,
                            true
                        ));

                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            LedgerConstants.StockLedgerId,
                            totalAmount,
                            0,
                            DateTime.Now,
                            companyId,
                            FyId,
                            true
                        ));
                        break;

                    case StockAdjustment.ReasonType.CountingError:
                        if (command.quantityChanged > 0)
                        {
                            // Stock Gain
                            journalDetails.Add(new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                LedgerConstants.StockLedgerId,
                                totalAmount,
                                0,
                                DateTime.Now,
                                companyId,
                                FyId,
                                true
                            ));

                            journalDetails.Add(new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                LedgerConstants.StockGainId,
                                0,
                                totalAmount,
                                DateTime.Now,
                                companyId,
                                FyId,
                                true
                            ));
                        }
                        else
                        {
                            // Stock Loss
                            journalDetails.Add(new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                LedgerConstants.StockLossLedger,
                                totalAmount,
                                0,
                                DateTime.Now,
                                companyId,
                                FyId,
                                true
                            ));

                            journalDetails.Add(new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                LedgerConstants.StockLedgerId,
                                0,
                                totalAmount,
                                DateTime.Now,
                                companyId,
                                FyId,
                                true
                            ));
                        }
                        break;

                    default:
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            LedgerConstants.MarketingAndAdvertisementLedger,
                            totalAmount,
                            0,
                            DateTime.Now,
                            companyId,
                            FyId,
                            true
                        ));

                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            LedgerConstants.StockLedgerId,
                            0,
                            totalAmount,
                            DateTime.Now,
                            companyId,
                            FyId,
                            true
                        ));
                        break;
                }





                #endregion

                var journalData = new JournalEntry
            (
                newJournalId,
                "Stock Adjustment",
                DateTime.Now,
                "Being Stock is adjusted with in given reason",
                _tokenService.GetUserId(),
                companyId,
                DateTime.Now,
                "",
                default,
                 "",
                 FyId,
                journalDetails,
                true

            );


                //if (journalDetails.Sum(x => x.DebitAmount) != journalDetails.Sum(x => x.CreditAmount))
                //{
                //    throw new InvalidOperationException("Journal entry is unbalanced.");
                //}
                await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);


                #endregion
                await _unitOfWork.SaveChangesAsync();

                var response = new AddStockAdjustmentResponse(
                    stockAdjustment.Id,
                    stockAdjustment.ItemId,
                    stockAdjustment.QuantityChanged,
                    stockAdjustment.Reason,
                   DateTime.UtcNow,
                    userId
                );

                return Result<AddStockAdjustmentResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<AddStockAdjustmentResponse>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var stockAdjustment = await _unitOfWork.BaseRepository<StockAdjustment>().GetByGuIdAsync(id);
                if (stockAdjustment is null)
                {
                    return Result<bool>.Failure("NotFound", "stock adjustment Cannot be Found");
                }
                _unitOfWork.BaseRepository<StockAdjustment>().Delete(stockAdjustment);
                await _unitOfWork.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Stock Adjustment having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllInventoriesLogsByQueryResponse>>> GetAllInventoriesLogs(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var inventoriesLogs = await _unitOfWork.BaseRepository<InventoriesLogs>().GetAllAsyncWithPagination();
                var inventoriesLogsPagedResult = await inventoriesLogs.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allInventoriesLogsResponse = _mapper.Map<PagedResult<GetAllInventoriesLogsByQueryResponse>>(inventoriesLogsPagedResult.Data);

                return Result<PagedResult<GetAllInventoriesLogsByQueryResponse>>.Success(allInventoriesLogsResponse);

            }
            catch (Exception ex)

            {
                throw new Exception("An error occurred while fetching all InventoriesLogs", ex);
            }

        }

        public async Task<Result<PagedResult<GetAllInventoryByQueryResponse>>> GetAllInventory(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var companyId = _tokenService.CompanyId().FirstOrDefault();
                var institutionId = _tokenService.InstitutionId() ?? "";
                var userRole = _tokenService.GetRole();
                bool isSuperAdmin = userRole == Role.SuperAdmin;

                List<string> companyIds = new();

                if (!isSuperAdmin && !string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(companyId))
                {
                    companyIds = (await _unitOfWork.BaseRepository<Company>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            query => query.Select(c => c.Id)
                        )).ToList();
                }

                IEnumerable<Inventories> filteredInventories;
                if (isSuperAdmin)
                {
                    filteredInventories = await _unitOfWork.BaseRepository<Inventories>()
                        .GetAllAsyncWithPagination(); // SuperAdmin  all
                }
                else if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(companyId))
                {
                    filteredInventories = await _unitOfWork.BaseRepository<Inventories>()
                        .FindBy(x =>x.IsActive && companyIds.Contains(x.companyId));
                }
                else
                {
                    filteredInventories = await _unitOfWork.BaseRepository<Inventories>()
                        .FindBy(x =>x.IsActive && x.companyId == companyId);
                }
                var itemIds = filteredInventories.Select(i => i.ItemId).Distinct().ToList();

                var items = (await _unitOfWork.BaseRepository<Items>()
                    .FindBy(i => itemIds.Contains(i.Id))).ToList();

                var groupedInventory = filteredInventories
                    .GroupBy(i => i.ItemId)
                    .Select(group =>
                    {
                        var item = items.FirstOrDefault(i => i.Id == group.Key);
                        return new GetAllInventoryByQueryResponse(
                            group.First().Id,
                            group.Key,
                            item?.Name ?? $"Item_{group.Key}",
                            group.Sum(i => i.QuantityIn) - group.Sum(i => i.QuantityOut),
                            group.Average(i => i.AmountIn),
                            group.Average(i => i.AmountOut),
                            group.Min(i => i.EntryDate),
                            group.Sum(i => i.QuantityOut)
                        );
                    }).ToList();

                var totalItems = groupedInventory.Count;

                var paginatedInventory = paginationRequest?.IsPagination == true
                    ? groupedInventory
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : groupedInventory;

                var pagedResult = new PagedResult<GetAllInventoryByQueryResponse>
                {
                    Items = paginatedInventory,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };

                await _stockAlertActivityProcessServices.ProcessStockExpiryAsync(cancellationToken);



                return Result<PagedResult<GetAllInventoryByQueryResponse>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Inventory", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllStockAdjustmentQueryResponse>>> GetAllStockAdjustment(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var companyId = _tokenService.CompanyId().FirstOrDefault();
                var institutionId = _tokenService.InstitutionId() ?? "";
                var userRoles = _tokenService.GetRole();

                var isSuperAdmin = userRoles == Role.SuperAdmin;

                IQueryable<StockAdjustment> stock = await _unitOfWork.BaseRepository<StockAdjustment>().GetAllAsyncWithPagination();

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                IQueryable<StockAdjustment> filteredStockAdjustment;

                if (isSuperAdmin)
                {
                    filteredStockAdjustment = stock;
                }
                else if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(companyId))
                {
                    filteredStockAdjustment = stock.Where(x => companyIds.Contains(x.CompanyId));
                }
                else
                {
                    filteredStockAdjustment = stock.Where(x => x.CompanyId == companyId);
                }

                filteredStockAdjustment = filteredStockAdjustment.OrderBy(x => x.ItemId);

                var pagedResult = await filteredStockAdjustment
                    .AsNoTracking()
                    .ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allResponse = _mapper.Map<PagedResult<GetAllStockAdjustmentQueryResponse>>(pagedResult.Data);

                return Result<PagedResult<GetAllStockAdjustmentQueryResponse>>.Success(allResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all stock adjustment", ex);
            }
        }

        public async Task<Result<PagedResult<FilterStockAdjustmentQueryResponse>>> GetFilterStockAdjustment(PaginationRequest paginationRequest, FilterStockAdjustmentDto filterStockAdjustmentDto, CancellationToken cancellationToken)
        {
            try
            {
                var (stockAdjustmentQueryable, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<StockAdjustment>();

               
                var filterStockAdjustments = isSuperAdmin
                    ? stockAdjustmentQueryable
                    : stockAdjustmentQueryable.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");

             
                DateTime? startEnglishDate = null;
                DateTime? endEnglishDate = null;

                if (!string.IsNullOrWhiteSpace(filterStockAdjustmentDto.startDate))
                    startEnglishDate = await _dateConvertHelper.ConvertToEnglish(filterStockAdjustmentDto.startDate);

                if (!string.IsNullOrWhiteSpace(filterStockAdjustmentDto.endDate))
                    endEnglishDate = await _dateConvertHelper.ConvertToEnglish(filterStockAdjustmentDto.endDate);

                // Default to today’s data if no date provided
                if (startEnglishDate == null && endEnglishDate == null)
                {
                    startEnglishDate = DateTime.Today;
                    endEnglishDate = DateTime.Today.AddDays(1).AddTicks(-1); // inclusive today
                }
                else if (startEnglishDate != null && endEnglishDate == null)
                {
                    endEnglishDate = startEnglishDate.Value.Date.AddDays(1).AddTicks(-1);
                }
                else if (endEnglishDate != null && startEnglishDate == null)
                {
                    startEnglishDate = endEnglishDate.Value.Date;
                    endEnglishDate = endEnglishDate.Value.Date.AddDays(1).AddTicks(-1);
                }
                else
                {
                  
                    endEnglishDate = endEnglishDate?.Date.AddDays(1).AddTicks(-1);
                }

                var userId = _tokenService.GetUserId();

              
                var stockAdjustmentResult = await _unitOfWork.BaseRepository<StockAdjustment>().GetConditionalAsync(
                    x =>
                        x.AdjustedBy == userId &&
                        (startEnglishDate == null || x.AdjustedAt >= startEnglishDate) &&
                        (endEnglishDate == null || x.AdjustedAt <= endEnglishDate) &&
                        (isSuperAdmin || x.CompanyId == companyId || x.CompanyId == "")
                );

                // Map to response
                var responseList = stockAdjustmentResult
                    .OrderByDescending(sa => sa.AdjustedAt)
                    .Select(sa => new FilterStockAdjustmentQueryResponse(
                        sa.Id,
                        sa.ItemId,
                        sa.QuantityChanged,
                        sa.Reason,
                        default,
                        userId
                    ))
                    .ToList();

                // Pagination
                PagedResult<FilterStockAdjustmentQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {
                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;
                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterStockAdjustmentQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterStockAdjustmentQueryResponse>
                    {
                        Items = responseList,
                        TotalItems = responseList.Count,
                        PageIndex = 1,
                        pageSize = responseList.Count
                    };
                }

                return Result<PagedResult<FilterStockAdjustmentQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Stock Adjustments: {ex.Message}", ex);
            }
        }

        public async Task<Result<GetInventoriesLogsByIdQueryResponse>> GetInventoriesLogsById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var inventoriesLogs = await _unitOfWork.BaseRepository<InventoriesLogs>().GetByGuIdAsync(id);

                var inventoriesLogsResponse = _mapper.Map<GetInventoriesLogsByIdQueryResponse>(inventoriesLogs);

                return Result<GetInventoriesLogsByIdQueryResponse>.Success(inventoriesLogsResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching InventoriesLogs by using Id", ex);
            }
        }

        public async Task<Result<FilterInventoryWithTotals>> GetInventoryFilter(PaginationRequest paginationRequest, FilterInventoryDtos filterInventoryDtos, CancellationToken cancellationToken)
        {
            try
            {
                DateTime startEnglishDate = filterInventoryDtos.startDate == null
                     ? DateTime.Today
                     : await _dateConvertHelper.ConvertToEnglish(filterInventoryDtos.startDate);

                DateTime endEnglishDate = filterInventoryDtos.endDate == null
                    ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(filterInventoryDtos.endDate);

                endEnglishDate = endEnglishDate.Date.AddDays(1).AddTicks(-1);

                // Step 1: Apply base filtering at DB level
                var filterInventory = await _unitOfWork.BaseRepository<Inventories>().GetConditionalAsync(
                    x =>x.IsActive && (string.IsNullOrEmpty(filterInventoryDtos.itemId) || x.ItemId.Contains(filterInventoryDtos.itemId)) &&
                         x.CreatedAt >= startEnglishDate &&
                         x.CreatedAt <= endEnglishDate
                );


                var companyId = _tokenService.CompanyId().FirstOrDefault();
                var institutionId = _tokenService.InstitutionId() ?? string.Empty;
                var isSuperAdmin = _tokenService.GetRole() == Role.SuperAdmin;


                IEnumerable<Inventories> filterInventories = isSuperAdmin
                    ? filterInventory
                    : filterInventory.Where(x =>x.IsActive && x.companyId == companyId);

                if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(companyId))
                {
                    var companyIds = await _unitOfWork.BaseRepository<Company>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            query => query.Select(c => c.Id)
                        );

                    filterInventories = filterInventories.Where(x => companyIds.Contains(x.companyId));
                }

                var itemIds = filterInventories.Select(x => x.ItemId).Distinct().ToList();
                var items = await _unitOfWork.BaseRepository<Items>().FindBy(i => itemIds.Contains(i.Id));
                var itemsDict = items.ToDictionary(i => i.Id, i => i); // Faster lookup

                var groupedInventory = filterInventories
                     .Where(i => i.Type == InventoriesType.None || i.Type == InventoriesType.Purchase)
                     .GroupBy(i => i.ItemId)
                     .Select(g =>
                     {
                         var item = itemsDict.ContainsKey(g.Key) ? itemsDict[g.Key] : null;

                         // FIX: use g (current group), not filterInventories
                         var priceList = g
                             .Where(i =>
                                 (i.Type == InventoriesType.Purchase && !string.IsNullOrEmpty(i.PurchaseItemsId)) ||
                                 (i.Type == InventoriesType.Sales && !string.IsNullOrEmpty(i.SalesItemsId)) ||
                                 (i.Type == InventoriesType.None && string.IsNullOrEmpty(i.SalesItemsId) && string.IsNullOrEmpty(i.PurchaseItemsId))
                             )
                             .Select(i => $"{i.QuantityIn}, {i.UnitId}, @{i.AmountIn}")
                             .ToList();


                         var totalValue = g.Sum(i => i.QuantityIn * i.AmountIn);
                         var totalQuantity = g.Sum(i => i.QuantityIn);


                         return new FilterInventoryByDateQueryResponse(
                             g.First().Id,
                             g.Key,
                             item?.Name ?? $"Item_{g.Key}",
                             totalQuantity,
                             g.Average(i => i.AmountIn),
                             g.Average(i => i.AmountOut),
                             g.Min(i => i.EntryDate),
                             g.Sum(i => i.QuantityOut),
                             priceList,
                             totalValue
                         //g.Average(i => i.AmountIn) - g.Average(i => i.AmountOut)
                         );
                     })
                     .ToList();


                var grandTotalQuantity = groupedInventory.Sum(x => x.remainingQuantity);
                var grandTotalValue = groupedInventory.Sum(x => x.totalValue);




                var totalItems = groupedInventory.Count();

                var paginatedLedgerBalance = paginationRequest != null && paginationRequest.IsPagination
                 ? groupedInventory
                     .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                     .Take(paginationRequest.pageSize)
                     .ToList()
                 : groupedInventory.ToList();

                var pagedResult = new PagedResult<FilterInventoryByDateQueryResponse>
                {
                    Items = paginatedLedgerBalance,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };

                var response = new FilterInventoryWithTotals(
                    PagedItems: pagedResult,
                    GrandTotalQuantity: grandTotalQuantity,
                    GrandTotalValue: grandTotalValue
                );





                return Result<FilterInventoryWithTotals>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching inventory by date: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<InventoryByItemQueryResponse>>> GetInventoryItem(string itemId, CancellationToken cancellationToken)
        {
            try
            {
                var itemInstances = await _unitOfWork.BaseRepository<ItemInstance>()
                     .GetConditionalAsync(
                         x => x.ItemsId == itemId,
                         query => query.Include(x => x.Items)
                     );


                var inventories = await _unitOfWork.BaseRepository<Inventories>()
                    .GetConditionalAsync(x => x.ItemId == itemId,
                    query => query.Include(x => x.Items)
                    );

                var openingSerials = itemInstances
                    .Where(i => string.IsNullOrEmpty(i.SalesItemsId) && string.IsNullOrEmpty(i.PurchaseItemsId))
                    .Select(i => i.SerialNumber)
                    .ToList();
                var result = inventories
               .OrderBy(inv => inv.EntryDate)
               .Select(inv =>
               {
                   var serialRateList = itemInstances
                       .Where(i =>
                           (inv.Type == Inventories.InventoriesType.Purchase && i.PurchaseItemsId == inv.PurchaseItemsId) ||
                           (inv.Type == Inventories.InventoriesType.Sales && i.SalesItemsId == inv.SalesItemsId) ||
                           (inv.Type == Inventories.InventoriesType.None) ||
                           (inv.IsOpeningStuck == true && string.IsNullOrEmpty(i.SalesItemsId) && string.IsNullOrEmpty(i.PurchaseItemsId))
                       )
                       .Select(i => i.SerialNumber ?? "N/A") // handle null serial numbers
                       .ToList();

                   // Safe parsing for SellingPrice
                   decimal sellingPrice = 0;
                   if (!string.IsNullOrWhiteSpace(inv.Items?.SellingPrice?.ToString()))
                   {
                       decimal.TryParse(inv.Items.SellingPrice.ToString(), out sellingPrice);
                   }

                   return new InventoryByItemQueryResponse(
                       inv.ItemId,
                       inv.EntryDate.ToString("yyyy-MM-dd"),
                       inv.QuantityOut,
                       sellingPrice,
                       inv.QuantityIn,
                       inv.AmountIn,
                       inv.AmountOut,
                       inv.LedgerId,
                       inv.Type,
                       serialRateList
                   );
               }).ToList();



                return Result<IEnumerable<InventoryByItemQueryResponse>>.Success(result);



            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Items by date: {ex.Message}");
            }
        }


        public async Task<Result<GetRemainingQtyByItemIdQueryResponse>> GetRemainingQtyByItemId(string itemId, CancellationToken cancellationToken = default)
        {
            try
            {
                var inventory = await _unitOfWork.BaseRepository<Inventories>().GetAllAsyncWithPagination();
                var filteredInventory = inventory.Where(i =>i.IsActive && i.ItemId == itemId);

                if (!filteredInventory.Any())
                {
                    return Result<GetRemainingQtyByItemIdQueryResponse>.Failure($"No inventory found for ItemId: {itemId}");
                }

                decimal totalQuantityIn = filteredInventory.Sum(i => i.QuantityIn);
                decimal totalQuantityOut = filteredInventory.Sum(i => i.QuantityOut);
                decimal RemainingQuantity = (totalQuantityIn - totalQuantityOut);

                var response = new GetRemainingQtyByItemIdQueryResponse(itemId, RemainingQuantity);

                return Result<GetRemainingQtyByItemIdQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching remaining quantity", ex);
            }
        }

        public async Task<Result<UpdateStockAdjustmentResponse>> UpdateStockAdjustment(string id, UpdateStockAdjustmentCommand command)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    
                    var userId = _tokenService.GetUserId();

                    if (string.IsNullOrEmpty(id))
                    {
                        return Result<UpdateStockAdjustmentResponse>.Failure("NotFound", "Please provide valid stock adjustment ID.");
                    }

                    var existingStockAdjustment = await _unitOfWork.BaseRepository<StockAdjustment>().GetByGuIdAsync(id);

                    if (existingStockAdjustment is null)
                    {
                        return Result<UpdateStockAdjustmentResponse>.Failure("NotFound", "Stock adjustment record not found.");
                    }
                    _mapper.Map(command, existingStockAdjustment);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var response = new UpdateStockAdjustmentResponse(
                        id,
                        existingStockAdjustment.ItemId,
                        existingStockAdjustment.QuantityChanged,
                        existingStockAdjustment.Reason,
                        DateTime.UtcNow,
                        userId
                    );

                    return Result<UpdateStockAdjustmentResponse>.Success(response);
                }
                catch (Exception ex)
                {
                    return Result<UpdateStockAdjustmentResponse>.Failure("Error", "An error occurred while updating stock adjustment.");
                }
            }


        }
    }
}
