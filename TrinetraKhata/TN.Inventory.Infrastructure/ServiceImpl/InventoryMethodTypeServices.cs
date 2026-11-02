

using TN.Account.Domain.Entities;

//using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Application.ServiceInterface;
using TN.Inventory.Domain.Entities;
using TN.Sales.Application.Sales.Command.AddSalesItems;
using TN.Sales.Domain.Entities;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using static TN.Authentication.Domain.Entities.CompanySettings;

namespace TN.Inventory.Infrastructure.ServiceImpl
{


    public class InventoryMethodTypeServices : IInventoryMethodType
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly FiscalContext _fiscalContext;


        public InventoryMethodTypeServices(IUnitOfWork unitOfWork, ITokenService tokenService, FiscalContext fiscalContext)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _fiscalContext = fiscalContext;
        }


        public void JournalCOGSEntry(decimal cogs, DateTime entryDate, string userId, string companyId)
        {
            try
            {
                string newJournalCOGSId = Guid.NewGuid().ToString();
                var FyId = _fiscalContext.CurrentFiscalYearId;


                var journalDetails = new List<JournalEntryDetails>
                {
                    new JournalEntryDetails(
                        Guid.NewGuid().ToString(),
                        newJournalCOGSId,
                        LedgerConstants.CostOfGoodSoldLedgerId,
                        cogs, // Debit amount
                        0,    // Credit amount
                        entryDate,
                        companyId,
                        _fiscalContext.CurrentFiscalYearId,
                        true
                    ),
                    new JournalEntryDetails(
                        Guid.NewGuid().ToString(),
                        newJournalCOGSId,
                        LedgerConstants.StockLedgerId,
                        0,    // Debit amount
                        cogs, // Credit amount
                        entryDate,
                        companyId,
                        _fiscalContext.CurrentFiscalYearId,
                        true
                    )
                };


                var journalData = new JournalEntry
                     (
                         newJournalCOGSId,
                         "COGS Vouchers",
                         entryDate,
                         "Being Item Sold by",
                         userId,
                         companyId,
                         DateTime.UtcNow,
                         "",
                         default,
                         "",
                         FyId,
                         journalDetails,
                         true
                     );

                if (journalDetails.Sum(x => x.DebitAmount) != journalDetails.Sum(x => x.CreditAmount))
                {
                    throw new InvalidOperationException("Journal entry is unbalanced.");
                }

                _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);



            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while entry cogs journal");
            }
        }

        public async Task<decimal> ProcessInventoryMethod(string companyId,string ledgerId, List<AddSalesItemsRequest> addSalesItemsRequests, SalesDetails salesDetails)
        {
            decimal totalCOGS = 0;
            try
            {
          

                var GetInventoryMethodType = await _unitOfWork.BaseRepository<CompanySettings>().GetSingleAsync(x => x.CompanyId == companyId);
                var itemIds = addSalesItemsRequests.Select(s => s.itemId).ToList();

                var existingSalesEntries = (await _unitOfWork.BaseRepository<Inventories>()
                    .GetConditionalAsync(i => itemIds.Contains(i.ItemId) && i.Type == Inventories.InventoriesType.Sales))
                    .ToDictionary(i => i.ItemId);

                var inventoryRecords = (await _unitOfWork.BaseRepository<Inventories>()
                    .GetConditionalAsync(i => itemIds.Contains(i.ItemId) && i.QuantityIn > 0))
                    .GroupBy(i => i.ItemId)
                    .ToDictionary(g => g.Key, g => g.ToList());


                var newSalesEntries = new List<Inventories>();
                var inventoryUpdates = new List<Inventories>();
                var inventoryDeletes = new List<Inventories>();
                var inventoryLogs = new List<InventoriesLogs>();

                foreach (var salesItem in addSalesItemsRequests)
                {
                    decimal totalAmountOut = salesItem.quantity * salesItem.price;
                    decimal quantityToConsume = salesItem.quantity;
                    decimal amountToConsume = totalAmountOut;

                    decimal itemCOGS = 0;

                    if (inventoryRecords.TryGetValue(salesItem.itemId, out var inventories))
                    {
                        decimal totalAvailableQuantity = inventories.Sum(i => i.QuantityIn);
                        if (totalAvailableQuantity < salesItem.quantity)
                        {
                            throw new Exception($"Insufficient inventory for item {salesItem.itemId}. Available: {totalAvailableQuantity}, Requested: {salesItem.quantity}");
                        }


                        List<Inventories> sortedInventories;
                        switch (GetInventoryMethodType.InventoryMethod)
                        {
                            case InventoryMethodType.FIFO:
                                sortedInventories = inventories.OrderBy(i => i.EntryDate).ToList(); // Oldest first
                                break;

                            case InventoryMethodType.LIFO:
                                sortedInventories = inventories.OrderByDescending(i => i.EntryDate).ToList(); // Newest first
                                break;

                            case InventoryMethodType.AverageWeighted:
                                decimal totalStock = inventories.Sum(i => i.QuantityIn);
                                decimal totalStockValue = inventories.Sum(i => i.AmountIn);
                                decimal avgCostPerUnit = totalStock > 0 ? totalStockValue / totalStock : 0;
                                amountToConsume = quantityToConsume * avgCostPerUnit;
                                sortedInventories = inventories.ToList(); // No order needed, only average cost matters
                                break;

                            default:
                                throw new Exception("Invalid inventory method selected.");
                        }

                        // **Process Inventory Consumption**
                        foreach (var inventory in sortedInventories)
                        {
                            if (quantityToConsume <= 0)
                                break;

                            if (inventory.QuantityIn <= quantityToConsume)
                            {
                                // Consume entire inventory row
                                quantityToConsume -= inventory.QuantityIn;
                                amountToConsume -= inventory.AmountIn;

                                itemCOGS += inventory.QuantityIn * (inventory.AmountIn / inventory.QuantityIn); // Compute batch COGS
                                //quantityToConsume -= inventory.QuantityIn;




                                // Move to logs before deletion
                                inventoryLogs.Add(new InventoriesLogs(
                                 Guid.NewGuid().ToString(),
                                 inventory.ItemId,
                                 inventory.QuantityIn,
                                 inventory.AmountIn,
                                 inventory.EntryDate,
                                 inventory.LedgerId,
                                 inventory.companyId,
                                 inventory.Type,
                                 DateTime.UtcNow,
                                 _tokenService.GetUserId(),
                                 ""
                             ));


                                inventoryDeletes.Add(inventory);
                            }
                            else
                            {
                                // Partially consume inventory
                                itemCOGS += quantityToConsume * (inventory.AmountIn / inventory.QuantityIn);
                                inventory.QuantityIn -= quantityToConsume;
                                inventory.AmountIn -= amountToConsume;
                                quantityToConsume = 0;
                                amountToConsume = 0;
                                inventoryUpdates.Add(inventory);
                            }
                        }

                        totalCOGS += itemCOGS;
                    }

                    // **Handle Sales Entry (Update Existing or Create New)**

                    var salesItemsId = salesDetails.SalesItems
                 .FirstOrDefault(x => x.ItemId == salesItem.itemId)?.Id;




                    if (existingSalesEntries.TryGetValue(salesItem.itemId, out var existingEntry))
                    {
                        existingEntry.QuantityOut += salesItem.quantity;
                        existingEntry.AmountOut += totalAmountOut;
                        existingEntry.SalesItemsId = salesItemsId;
                        inventoryUpdates.Add(existingEntry);
                    }
                    else
                    {
                        var newEntry = new Inventories(
                            Guid.NewGuid().ToString(),
                            salesItem.itemId,
                            0,  // No QuantityIn for sales
                            0,
                            DateTime.UtcNow,
                            salesItem.quantity,
                            totalAmountOut,
                            ledgerId,
                            false,
                            Inventories.InventoriesType.Sales,
                            companyId,
                            salesItem.unitId,
                            "",
                            salesItemsId,
                            salesDetails.StockCenterId,
                             "",
                            DateTime.UtcNow,
                            "",
                            default,
                            true
                        );

                        newSalesEntries.Add(newEntry);
                    }


                }

                // **Perform batch operations**
                if (inventoryUpdates.Any())
                    await _unitOfWork.BaseRepository<Inventories>().UpdateRange(inventoryUpdates);

                if (inventoryDeletes.Any())
                    _unitOfWork.BaseRepository<Inventories>().DeleteRange(inventoryDeletes);

                if (newSalesEntries.Any())
                    await _unitOfWork.BaseRepository<Inventories>().AddRange(newSalesEntries);

                if (inventoryLogs.Any())
                    await _unitOfWork.BaseRepository<InventoriesLogs>().AddRange(inventoryLogs);

            }
            catch (Exception)
            {
                throw;
            }

            return totalCOGS;
        }
    }
}
