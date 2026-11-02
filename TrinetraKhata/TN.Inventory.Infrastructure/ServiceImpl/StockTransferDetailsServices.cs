using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Application.Inventory.Command.AddStockTransferDetails;
using TN.Inventory.Application.Inventory.Command.AddStockTransferItems;
using TN.Inventory.Application.Inventory.Command.UpdateStockTransferDetails;
using TN.Inventory.Application.Inventory.Queries.FilterStockTransferDetails;
using TN.Inventory.Application.Inventory.Queries.GetAllStockTransferDetails;
using TN.Inventory.Application.Inventory.Queries.GetStockTransferDetailsById;
using TN.Inventory.Application.ServiceInterface;
using TN.Inventory.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Inventory;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace TN.Inventory.Infrastructure.ServiceImpl
{
    public class StockTransferDetailsServices : IStockTransferDetailsServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConvertHelper;

        public StockTransferDetailsServices(IUnitOfWork unitOfWork, IDateConvertHelper dateConvertHelper, IGetUserScopedData getUserScopedData, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _getUserScopedData = getUserScopedData;
            _dateConvertHelper = dateConvertHelper;
        }
        public async Task<Result<AddStockTransferDetailsResponse>> Add(AddStockTransferCommand command)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                string newId = Guid.NewGuid().ToString();
                string companyId = _tokenService.CompanyId().FirstOrDefault() ?? "";
                string userId = _tokenService.GetUserId();

                DateTime entryDate = command.transferDate == null
                    ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(command.transferDate);

                string nepaliDate = await _dateConvertHelper.ConvertToNepali(entryDate);

                // --- Create StockTransferDetails entity ---
                var stockTransferDetails = new StockTransferDetails(
                    newId,
                    nepaliDate,
                    command.stockCenterNumber,
                    command.fromStockCenterId,
                    command.toStockCenterId,
                    command.narration,
                    userId,
                    DateTime.Now,
                    "",
                    default,
                    companyId,
                    command.addStockTransferItemsRequests?.Select(d => new StockTransferItems(
                        Guid.NewGuid().ToString(),
                        d.quantity,
                        d.unitId,
                        d.itemId,
                        d.price,
                        d.amount,
                        newId,
                        userId,
                        DateTime.Now.ToString(),
                        "",
                        default
                    )).ToList() ?? new List<StockTransferItems>()
                );

                await _unitOfWork.BaseRepository<StockTransferDetails>().AddAsync(stockTransferDetails);

                // --- Update Stock Center Stock ---
                var itemIds = command.addStockTransferItemsRequests?.Select(x => x.itemId).ToList() ?? new List<string>();

                var stockOutItems = await _unitOfWork.BaseRepository<Items>()
                    .GetConditionalAsync(s =>
                        s.StockCenterId == command.fromStockCenterId &&
                        itemIds.Contains(s.Id)
                    );

                foreach (var request in command.addStockTransferItemsRequests)
                {
                    var stockOutItem = stockOutItems.FirstOrDefault(x => x.Id == request.itemId);
                    if (stockOutItem == null) continue;

                    if (stockOutItem.OpeningStockQuantity < request.quantity)
                        return Result<AddStockTransferDetailsResponse>.Failure("Error", $"Not enough stock for Item {stockOutItem.Name}");

                    stockOutItem.OpeningStockQuantity -= request.quantity;

                    if (stockOutItem.OpeningStockQuantity == 0)
                        _unitOfWork.BaseRepository<Items>().Delete(stockOutItem);
                    else
                        _unitOfWork.BaseRepository<Items>().Update(stockOutItem);

                    var stockInItem = await _unitOfWork.BaseRepository<Items>()
                        .GetSingleAsync(x => x.StockCenterId == command.toStockCenterId && x.Id == request.itemId);


                    if (stockInItem != null)
                    {
                        stockInItem.OpeningStockQuantity += request.quantity;
                        _unitOfWork.BaseRepository<Items>().Update(stockInItem);
                    }
                    else
                    {
                        var newItem = new Items(
                            id: Guid.NewGuid().ToString(),
                            name: stockOutItem.Name,
                            price: stockOutItem.Price,
                            itemGroupId: stockOutItem.ItemGroupId,
                            unitId: stockOutItem.UnitId,
                            sellingPrice: stockOutItem.SellingPrice,
                            costPrice: stockOutItem.CostPrice,
                            barCodeField: stockOutItem.BarCodeField,
                            openingStockQuantity: request.quantity,
                            hsCode: stockOutItem.HsCode,
                            companyId: stockOutItem.CompanyId,
                            createdBy: userId,
                            createdAt: DateTime.Now,
                            modifiedBy: "",
                            modifiedAt: DateTime.Now,
                            minimumLevel: stockOutItem.MinimumLevel,
                            hasSerial: stockOutItem.HasSerial,
                            conversionFactorId: stockOutItem.ConversionFactorId,
                            isItems: stockOutItem.IsItems,
                            isVatEnables: stockOutItem.IsVatEnables,
                            isConversionFactor: stockOutItem.IsConversionFactor,
                            stockCenterId: command.toStockCenterId,
                            hasExpiryAndManufacture: stockOutItem.HasExpiryAndManufacture,
                            hasBatchNumber: stockOutItem.HasBatchNumber,
                            manufactureAndExpiries: null,
                            batchNumbers: null

                        );

                        await _unitOfWork.BaseRepository<Items>().AddAsync(newItem);
                    }
                }

                // --- Update Inventory Stock ---
                var inventoriesOutItems = await _unitOfWork.BaseRepository<Inventories>()
                    .GetConditionalAsync(s =>
                        s.StockCenterId == command.fromStockCenterId &&
                        itemIds.Contains(s.ItemId)
                    );

                foreach (var request in command.addStockTransferItemsRequests)
                {
                    var inventoriesOutItem = inventoriesOutItems.LastOrDefault(x => x.ItemId == request.itemId);
                    if (inventoriesOutItem == null) continue;

                    if (inventoriesOutItem.QuantityIn < request.quantity)
                        return Result<AddStockTransferDetailsResponse>.Failure("Error", $"Not enough inventory for Item {inventoriesOutItem.ItemId}");

                    inventoriesOutItem.QuantityIn -= request.quantity;

                    if (inventoriesOutItem.QuantityIn == 0)
                        _unitOfWork.BaseRepository<Inventories>().Delete(inventoriesOutItem);
                    else
                        _unitOfWork.BaseRepository<Inventories>().Update(inventoriesOutItem);

                    var inventoriesInItem = await _unitOfWork.BaseRepository<Inventories>()
                        .GetSingleAsync(x => x.StockCenterId == command.toStockCenterId && x.ItemId == request.itemId);

                    if (inventoriesInItem != null)
                    {
                        inventoriesInItem.QuantityIn += request.quantity;
                        _unitOfWork.BaseRepository<Inventories>().Update(inventoriesInItem);
                    }
                    else
                    {
                        var stockOutInventory = new Inventories(
                            id: Guid.NewGuid().ToString(),
                            itemId: inventoriesOutItem.ItemId,
                            quantityIn: 0,
                            amountIn: 0,
                            entryDate: DateTime.Now,
                            quantityOut: request.quantity,
                            amountOut: inventoriesOutItem.AmountIn * request.quantity,
                            ledgerId: "",
                            isOpeningStock: false,
                            type: Inventories.InventoriesType.None,
                            CompanyId: companyId,
                            unitId: inventoriesOutItem.UnitId,
                            purchaseItemsId: "",
                            salesItemsId: null,
                            stockCenterId: command.fromStockCenterId,
                            createdBy: userId,
                            createdAt: DateTime.Now,
                            modifiedBy: userId,
                            modifiedAt: DateTime.Now,
                            isActive: true
                        );
                        await _unitOfWork.BaseRepository<Inventories>().AddAsync(stockOutInventory);
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                scope.Complete();

                // --- Map response manually ---
                var resultDTOs = new AddStockTransferDetailsResponse(
                  
                    transferDate: stockTransferDetails.TransferDate,
                    stockCenterNumber: stockTransferDetails.StockCenterNumber,
                    fromStockCenterId: stockTransferDetails.FromStockCenterId,
                    toStockCenterId: stockTransferDetails.ToStockCenterId,
                    narration:stockTransferDetails.Narration,
                    addStockTransferItemsRequests: stockTransferDetails.StockTransferItems?
                        .Select(item => new AddStockTransferItemsRequest(
                            quantity: item.Quantity,
                            unitId: item.UnitId,
                            itemId: item.ItemId,
                            price: item.Price,
                            amount: item.Amount
                        )).ToList() ?? new List<AddStockTransferItemsRequest>()
                );

                return Result<AddStockTransferDetailsResponse>.Success(resultDTOs);
            }
            catch (Exception ex)
            {
                return Result<AddStockTransferDetailsResponse>.Failure("Error", ex.Message);
            }
        }
        

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                
                var stockTransferDetailsList = await _unitOfWork.BaseRepository<StockTransferDetails>()
               .GetConditionalAsync(
                   x => x.Id == id,   
                   query => query,   
                   x => x.StockTransferItems 
               );

                var stockTransferDetails = stockTransferDetailsList.FirstOrDefault();

                if (stockTransferDetails is null)
                {
                    return Result<bool>.Failure("NotFound", "StockTransferDetails cannot be found");
                }

               
                if (stockTransferDetails.StockTransferItems != null && stockTransferDetails.StockTransferItems.Any())
                {
                    _unitOfWork.BaseRepository<StockTransferItems>()
                        .DeleteRange(stockTransferDetails.StockTransferItems.ToList());
                }

          
                _unitOfWork.BaseRepository<StockTransferDetails>().Delete(stockTransferDetails);

                
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting StockTransferDetails: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterStockTransferQueryResponse>>> FilterStockTransferDetails(PaginationRequest paginationRequest, FilterStockTransferDetailsDto filterStockTransferDetailsDto)
        {
            try
            {
                var (stockTransfers, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<StockTransferDetails>();

                var currentCompanyId = _tokenService.CompanyId().FirstOrDefault();
                var filterStockCenter = isSuperAdmin
                    ? stockTransfers
                    : stockTransfers.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");

                DateTime today = DateTime.Today;
                DateTime? startEnglishDate = null;
                DateTime? endEnglishDate = null;

               
                if (filterStockTransferDetailsDto.startDate != default)
                    startEnglishDate = await _dateConvertHelper.ConvertToEnglish(filterStockTransferDetailsDto.startDate);

                if (filterStockTransferDetailsDto.endDate != default)
                    endEnglishDate = await _dateConvertHelper.ConvertToEnglish(filterStockTransferDetailsDto.endDate);

                // Case 1: No dates → today only
                if (startEnglishDate == null && endEnglishDate == null)
                {
                    startEnglishDate = today;
                    endEnglishDate = today.AddDays(1).AddTicks(-1);
                }
                // Case 2: Only start date → take full day
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
                    startEnglishDate = startEnglishDate.Value.Date;
                    endEnglishDate = endEnglishDate.Value.Date.AddDays(1).AddTicks(-1);
                }

                var result = await _unitOfWork.BaseRepository<StockTransferDetails>().GetConditionalAsync(
                    x => x.CompanyId == currentCompanyId &&
                        (startEnglishDate == null || x.CreatedAt >= startEnglishDate) &&
                        (endEnglishDate == null || x.CreatedAt <= endEnglishDate),
                    q => q.Include(sd => sd.StockTransferItems)
                );

           
                var responseList = result.Select(i => new FilterStockTransferQueryResponse(
                    i.Id,
                    i.TransferDate,
                    i.StockCenterNumber,
                    i.FromStockCenterId,
                    i.ToStockCenterId,
                    i.Narration,
                    i.StockTransferItems.Select(d => new AddStockTransferItemsRequest(
                        d.Quantity,
                        d.UnitId,
                        d.ItemId,
                        d.Price,
                        d.Amount
                    )).ToList()
                )).ToList();

               
                responseList = responseList
                    .OrderByDescending(r => r.transferDate == today.ToString("yyyy-MM-dd")) // today first
                    .ThenByDescending(r => r.transferDate) 
                    .ToList();

               
                PagedResult<FilterStockTransferQueryResponse> finalResponseList;
                if (paginationRequest.IsPagination)
                {
                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count;

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterStockTransferQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterStockTransferQueryResponse>
                    {
                        Items = responseList,
                        TotalItems = responseList.Count,
                        PageIndex = 1,
                        pageSize = responseList.Count
                    };
                }

                return Result<PagedResult<FilterStockTransferQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Stock TransferDetails: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllStockTransferDetailsQueryResponse>>> GetAllStockTransferDetail(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var stockTransfers = await _unitOfWork.BaseRepository<StockTransferDetails>()
                    .GetConditionalAsync(
                        st => true, 
                        query => query.Include(st => st.StockTransferItems)
                    ) ?? new List<StockTransferDetails>();

                var stockTransferResponses = stockTransfers.Select(st => new GetAllStockTransferDetailsQueryResponse(
                    st.Id,
                    st.TransferDate,
                    st.StockCenterNumber,
                    st.FromStockCenterId,
                    st.ToStockCenterId,
                    st.Narration,
                    st.StockTransferItems?.Select(item => new AddStockTransferItemsRequest(
                        item.Quantity,
                        item.UnitId,
                        item.ItemId,
                        item.Price,
                        item.Amount
                    )).ToList() ?? new List<AddStockTransferItemsRequest>()
                )).ToList();

                PagedResult<GetAllStockTransferDetailsQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {
                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = stockTransferResponses.Count;

                    var pagedItems = stockTransferResponses
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetAllStockTransferDetailsQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetAllStockTransferDetailsQueryResponse>
                    {
                        Items = stockTransferResponses,
                        TotalItems = stockTransferResponses.Count,
                        PageIndex = 1,
                        pageSize = stockTransferResponses.Count
                    };
                }

                return Result<PagedResult<GetAllStockTransferDetailsQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all stock transfer details", ex);
            }
        }

        public async Task<Result<GetStockTransferDetailsByIdQueryResponse>> GetStockTransferDetailsById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var purchaseDetails = await _unitOfWork.BaseRepository<StockTransferDetails>()
                     .GetConditionalAsync(
                         x => x.Id == id,
                         query => query
                             .Include(pd => pd.StockTransferItems)
                                 .ThenInclude(pi => pi.Item)              
                                    
                     );
               

                var transfer = purchaseDetails.FirstOrDefault();
                var detailsResponse = new GetStockTransferDetailsByIdQueryResponse(
                    transfer.Id,
                    transfer.TransferDate,
                    transfer.StockCenterNumber,
                    transfer.FromStockCenterId,
                    transfer.ToStockCenterId,
                    transfer.Narration,
                    transfer.StockTransferItems?.Select(detail => new AddStockTransferItemsRequest(
                        detail.Quantity,
                        detail.UnitId,
                        detail.ItemId,
                        detail.Price,
                        detail.Amount
                    )).ToList() ?? new List<AddStockTransferItemsRequest>()
                );


                return Result<GetStockTransferDetailsByIdQueryResponse>.Success(detailsResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Stock Transfer details by id", ex);

            }
        }

        public async Task<Result<UpdateStockTransferDetailsResponse>> Update(string id, UpdateStockTransferDetailsCommand command)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                if (string.IsNullOrEmpty(id))
                    return Result<UpdateStockTransferDetailsResponse>.Failure("InvalidRequest", "StockTransferDetails ID cannot be null or empty");

                string companyId = _tokenService.CompanyId().FirstOrDefault() ?? "";
                string userId = _tokenService.GetUserId();

                // --- Fetch StockTransferDetails ---
                var stockTransferDetails = (await _unitOfWork.BaseRepository<StockTransferDetails>()
                    .GetConditionalAsync(
                        x => x.Id == id,
                        q => q.Include(s => s.StockTransferItems)))
                    .FirstOrDefault();

                if (stockTransferDetails is null)
                    return Result<UpdateStockTransferDetailsResponse>.Failure("NotFound", "StockTransferDetails not found");

                // --- Update basic info ---
                DateTime entryDate = command.transferDate == null
                    ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(command.transferDate);
                string nepaliDate = await _dateConvertHelper.ConvertToNepali(entryDate);

                stockTransferDetails.TransferDate = nepaliDate;
                stockTransferDetails.StockCenterNumber = command.stockCenterNumber;
                stockTransferDetails.FromStockCenterId = command.fromStockCenterId;
                stockTransferDetails.ToStockCenterId = command.toStockCenterId;
                stockTransferDetails.Narration = command.narration;
                stockTransferDetails.UpdatedAt = DateTime.UtcNow;
                stockTransferDetails.UpdatedBy = userId;

                // --- Reverse previous stock & inventory changes ---
                var previousItems = stockTransferDetails.StockTransferItems.ToList();

                foreach (var item in previousItems)
                {
                    // Reverse Stock Center Stock
                    var fromStockItem = await _unitOfWork.BaseRepository<Items>()
                        .GetSingleAsync(x => x.StockCenterId == stockTransferDetails.FromStockCenterId && x.Id == item.ItemId);

                    if (fromStockItem != null)
                        fromStockItem.OpeningStockQuantity += item.Quantity;
                    else
                    {
                        string newItemId = Guid.NewGuid().ToString();
                        // If item was deleted, recreate in From Stock Center
                        await _unitOfWork.BaseRepository<Items>().AddAsync(new Items(
                            id: newItemId,
                            name: item.Item.Name,
                            price: item.Price,
                            itemGroupId: item.Item.ItemGroupId,
                            unitId: item.UnitId,
                            sellingPrice: item.Item.SellingPrice,
                            costPrice: item.Item.CostPrice,
                            barCodeField: item.Item.BarCodeField,
                            openingStockQuantity: item.Quantity,
                            hsCode: item.Item.HsCode,
                            companyId: companyId,
                            createdBy: userId,
                            createdAt: DateTime.Now,
                            modifiedBy: "",
                            modifiedAt: DateTime.Now,
                            minimumLevel: item.Item.MinimumLevel,
                            hasSerial: item.Item.HasSerial,
                            conversionFactorId: item.Item.ConversionFactorId,
                            isItems: item.Item.IsItems,
                            isVatEnables: item.Item.IsVatEnables,
                            isConversionFactor: item.Item.IsConversionFactor,
                            stockCenterId: stockTransferDetails.FromStockCenterId,
                            hasExpiryAndManufacture: item.Item.HasExpiryAndManufacture,
                            hasBatchNumber: item.Item.HasBatchNumber,
                            manufactureAndExpiries: item.Item.ManufacturingAndExpiries?.Select(x=>new ManufactureAndExpiry(Guid.NewGuid().ToString(), x.ExpiredDate, x.ManufacturingDate,x.TotalQuantity, newItemId)).ToList(),
                            batchNumbers: item.Item.BatchNumbers?.Select(x => new BatchNumber(Guid.NewGuid().ToString(), x.BatchNumbers,x.TotalQuantity, newItemId)).ToList()
                        ));
                    }

                    var toStockItem = await _unitOfWork.BaseRepository<Items>()
                        .GetSingleAsync(x => x.StockCenterId == stockTransferDetails.ToStockCenterId && x.Id == item.ItemId);

                    if (toStockItem != null)
                    {
                        toStockItem.OpeningStockQuantity -= item.Quantity;
                        if (toStockItem.OpeningStockQuantity <= 0)
                            _unitOfWork.BaseRepository<Items>().Delete(toStockItem);
                        else
                            _unitOfWork.BaseRepository<Items>().Update(toStockItem);
                    }
                }

                // --- Delete old StockTransferItems ---
                _unitOfWork.BaseRepository<StockTransferItems>().DeleteRange(previousItems);

                // --- Add new StockTransferItems & update stock ---
                if (command.addStockTransferItemsRequests?.Any() == true)
                {
                    foreach (var request in command.addStockTransferItemsRequests)
                    {
                        var newItem = new StockTransferItems(
                            Guid.NewGuid().ToString(),
                            request.quantity,
                            request.unitId,
                            request.itemId,
                            request.price,
                            request.amount,
                            stockTransferDetails.Id,
                            userId,
                            DateTime.Now.ToString(),
                            "",
                            default
                        );

                        stockTransferDetails.StockTransferItems.Add(newItem);

                        // Update From Stock Center
                        var stockOutItem = await _unitOfWork.BaseRepository<Items>()
                            .GetSingleAsync(x => x.StockCenterId == command.fromStockCenterId && x.Id == request.itemId);

                        if (stockOutItem != null)
                        {
                            if (stockOutItem.OpeningStockQuantity < request.quantity)
                                return Result<UpdateStockTransferDetailsResponse>.Failure("Error", $"Not enough stock for Item {request.itemId}");

                            stockOutItem.OpeningStockQuantity -= request.quantity;
                            if (stockOutItem.OpeningStockQuantity == 0)
                                _unitOfWork.BaseRepository<Items>().Delete(stockOutItem);
                            else
                                _unitOfWork.BaseRepository<Items>().Update(stockOutItem);
                        }

                        // Update To Stock Center
                        var stockInItem = await _unitOfWork.BaseRepository<Items>()
                            .GetSingleAsync(x => x.StockCenterId == command.toStockCenterId && x.Id == request.itemId);

                        if (stockInItem != null)
                        {
                            stockInItem.OpeningStockQuantity += request.quantity;
                            _unitOfWork.BaseRepository<Items>().Update(stockInItem);
                        }
                        else
                        {
                            var stockOutItemForCopy = await _unitOfWork.BaseRepository<Items>()
                                .GetSingleAsync(x => x.Id == request.itemId);

                            if (stockOutItemForCopy != null)
                            {
                               string newItemId = Guid.NewGuid().ToString();
                                var stockInNewItem = new Items(
                                    id: newItemId,
                                    name: stockOutItemForCopy.Name,
                                    price: stockOutItemForCopy.Price,
                                    itemGroupId: stockOutItemForCopy.ItemGroupId,
                                    unitId: stockOutItemForCopy.UnitId,
                                    sellingPrice: stockOutItemForCopy.SellingPrice,
                                    costPrice: stockOutItemForCopy.CostPrice,
                                    barCodeField: stockOutItemForCopy.BarCodeField,
                      
                                    openingStockQuantity: request.quantity,
                                    hsCode: stockOutItemForCopy.HsCode,
                                    companyId: companyId,
                                    createdBy: userId,
                                    createdAt: DateTime.Now,
                                    modifiedBy: "",
                                    modifiedAt: DateTime.Now,
                                    minimumLevel: stockOutItemForCopy.MinimumLevel,
                                    hasSerial: stockOutItemForCopy.HasSerial,
                                    conversionFactorId: stockOutItemForCopy.ConversionFactorId,
                                    isItems: stockOutItemForCopy.IsItems,
                                    isVatEnables: stockOutItemForCopy.IsVatEnables,
                                    isConversionFactor: stockOutItemForCopy.IsConversionFactor,
             
                                    stockCenterId: command.toStockCenterId,
                                    hasExpiryAndManufacture: stockOutItemForCopy.HasExpiryAndManufacture,
                                    hasBatchNumber: stockOutItemForCopy.HasBatchNumber,
                                                  manufactureAndExpiries: stockOutItemForCopy.ManufacturingAndExpiries?.Select(x => new ManufactureAndExpiry(Guid.NewGuid().ToString(), x.ExpiredDate, x.ManufacturingDate,x.TotalQuantity, newItemId)).ToList(),
                            batchNumbers: stockOutItemForCopy.BatchNumbers?.Select(x => new BatchNumber(Guid.NewGuid().ToString(), x.BatchNumbers,x.TotalQuantity, newItemId)).ToList()


                                );

                                await _unitOfWork.BaseRepository<Items>().AddAsync(stockInNewItem);
                            }
                        }
                    }
                }

                // --- Save Changes ---
                await _unitOfWork.SaveChangesAsync();
                scope.Complete();

                var resultDTO = new UpdateStockTransferDetailsResponse(
                    stockTransferDetails.TransferDate,
                    stockTransferDetails.StockCenterNumber,
                    stockTransferDetails.FromStockCenterId,
                    stockTransferDetails.ToStockCenterId,
                    stockTransferDetails.Narration,
                    stockTransferDetails.StockTransferItems
                        .Select(x => new AddStockTransferItemsRequest(
                            x.Quantity, x.UnitId, x.ItemId, x.Price, x.Amount))
                        .ToList()
                );

                return Result<UpdateStockTransferDetailsResponse>.Success(resultDTO);
            }
            catch (Exception ex)
            {
                scope.Dispose();
                return Result<UpdateStockTransferDetailsResponse>.Failure("Error", ex.Message);
            }
        }
    }
}


