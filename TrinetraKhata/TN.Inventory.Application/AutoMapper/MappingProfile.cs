using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.AddConversionFactor;
using TN.Inventory.Application.Inventory.Command.AddItemGroup;
using TN.Inventory.Application.Inventory.Command.AddItems;
using TN.Inventory.Application.Inventory.Command.AddStockAdjustment;
using TN.Inventory.Application.Inventory.Command.AddStockCenter;
using TN.Inventory.Application.Inventory.Command.AddStockTransferDetails;
using TN.Inventory.Application.Inventory.Command.AddStockTransferItems;
using TN.Inventory.Application.Inventory.Command.AddUnits;
using TN.Inventory.Application.Inventory.Command.DeleteConversionFactor;
using TN.Inventory.Application.Inventory.Command.DeleteItem;
using TN.Inventory.Application.Inventory.Command.DeleteItemGroup;
using TN.Inventory.Application.Inventory.Command.DeleteUnits;
using TN.Inventory.Application.Inventory.Command.UpdateConversionFactor;
using TN.Inventory.Application.Inventory.Command.UpdateItem;
using TN.Inventory.Application.Inventory.Command.UpdateItemGroup;
using TN.Inventory.Application.Inventory.Command.UpdateStockAdjustment;
using TN.Inventory.Application.Inventory.Command.UpdateStockCenter;
using TN.Inventory.Application.Inventory.Command.UpdateStockTransferDetails;
using TN.Inventory.Application.Inventory.Command.UpdateUnits;
using TN.Inventory.Application.Inventory.Queries.ConversionFactor;
using TN.Inventory.Application.Inventory.Queries.ConversionFactorById;
using TN.Inventory.Application.Inventory.Queries.FilterInventoryByDate;
using TN.Inventory.Application.Inventory.Queries.GetAllInventory;
using TN.Inventory.Application.Inventory.Queries.GetAllInventoryLogs;
using TN.Inventory.Application.Inventory.Queries.GetAllStockAdjustment;
using TN.Inventory.Application.Inventory.Queries.GetAllStockTransferDetails;
using TN.Inventory.Application.Inventory.Queries.GetRemainingQtyByItemId;
using TN.Inventory.Application.Inventory.Queries.GetStockTransferDetailsById;
using TN.Inventory.Application.Inventory.Queries.ItemGroup;
using TN.Inventory.Application.Inventory.Queries.ItemGroupById;
using TN.Inventory.Application.Inventory.Queries.Items;
using TN.Inventory.Application.Inventory.Queries.ItemsById;
using TN.Inventory.Application.Inventory.Queries.StockCenters;
using TN.Inventory.Application.Inventory.Queries.StockCentersById;
using TN.Inventory.Application.Inventory.Queries.StockExpiryNotification;
using TN.Inventory.Application.Inventory.Queries.Units;
using TN.Inventory.Application.Inventory.Queries.UnitsById;
using TN.Inventory.Domain.Entities;
using TN.Shared.Domain.Entities.Inventory;
using TN.Shared.Domain.Entities.Notification;
using TN.Shared.Domain.Entities.StockCenterEntities;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region StockExpiryNotification

            CreateMap<StockExpiryNotificationResponse, StockExpiryNotification>().ReverseMap();
            CreateMap<PagedResult<StockExpiryNotification>, PagedResult<StockExpiryNotificationResponse>>().ReverseMap();
            #endregion
            #region Units
            CreateMap<GetAllUnitsByQueryResponse, Units>().ReverseMap();
            CreateMap<PagedResult<Units>, PagedResult<GetAllUnitsByQueryResponse>>().ReverseMap();
            CreateMap<GetUnitsByIdQueryResponse, Units>().ReverseMap();
            CreateMap<AddUnitsResponse, Units>().ReverseMap();
            CreateMap<AddUnitsCommand, AddUnitsResponse>().ReverseMap();
            CreateMap<Units, DeleteUnitsCommand>().ReverseMap();
            CreateMap<Units, UpdateUnitsCommand>().ReverseMap();
            #endregion

            #region ConversionFactor
            CreateMap<GetAllConversionFactorQueryResponse, ConversionFactor>().ReverseMap();
            CreateMap<PagedResult<ConversionFactor>, PagedResult<GetAllConversionFactorQueryResponse>>().ReverseMap();
            CreateMap<GetConversionFactorByIdResponse, ConversionFactor>().ReverseMap();
            CreateMap<AddConversionFactorResponse, ConversionFactor>().ReverseMap();
            CreateMap<AddConversionFactorCommand, AddConversionFactorResponse>();
            CreateMap<ConversionFactor, DeleteConversionFactorCommand>().ReverseMap();
            CreateMap<ConversionFactor, UpdateConversionFactorCommand>().ReverseMap();
            #endregion

            #region ItemGroup
            CreateMap<GetAllItemGroupByQueryResponse, ItemGroup>().ReverseMap();
            CreateMap<PagedResult<ItemGroup>, PagedResult<GetAllItemGroupByQueryResponse>>().ReverseMap();
            CreateMap<GetItemGroupByIdQueryResponse, ItemGroup>().ReverseMap();
            CreateMap<AddItemGroupResponse, ItemGroup>().ReverseMap();
            CreateMap<AddItemGroupCommand, AddItemGroupResponse>();
            CreateMap<Units, DeleteItemGroupCommand>().ReverseMap();
            CreateMap<ItemGroup, UpdateItemGroupCommand>().ReverseMap();
            #endregion

            #region Items
            CreateMap<GetAllItemByQueryResponse, Items>().ReverseMap();
            CreateMap<PagedResult<Items>, PagedResult<GetAllItemByQueryResponse>>().ReverseMap();
            CreateMap<GetItemByIdResponse, Items>().ReverseMap();
            CreateMap<AddItemResponse, Items>().ReverseMap();
            CreateMap<AddItemCommand, AddItemResponse>();
            CreateMap<Items, DeleteItemCommand>().ReverseMap();
            CreateMap<Items, UpdateItemCommand>().ReverseMap();
            CreateMap<AddItemRequest, AddItemCommand>();
            CreateMap<UpdateItemCommand, Items>().ReverseMap();
            #endregion

            #region Inventories
            CreateMap<GetAllInventoryByQueryResponse, Inventories>().ReverseMap();
            CreateMap<PagedResult<Inventories>, PagedResult<GetAllInventoryByQueryResponse>>().ReverseMap();

            CreateMap<Inventories, GetRemainingQtyByItemIdQueryResponse>()
           .ForMember(dest => dest.RemainingQuantity, opt => opt.MapFrom(src => (src.QuantityIn - src.QuantityOut).ToString()));

            CreateMap<GetAllInventoriesLogsByQueryResponse, InventoriesLogs>().ReverseMap();
            CreateMap<PagedResult<InventoriesLogs>, PagedResult<GetAllInventoriesLogsByQueryResponse>>().ReverseMap();


            CreateMap<FilterInventoryByDateQueryResponse, Inventories>().ReverseMap();
            CreateMap<PagedResult<Inventories>, PagedResult<FilterInventoryByDateQueryResponse>>().ReverseMap();

           

            #endregion

            #region StockCenter
            CreateMap<GetAllStockCenterQueryResponse, StockCenter>().ReverseMap();
            CreateMap<PagedResult<StockCenter>, PagedResult<GetAllStockCenterQueryResponse>>().ReverseMap();
            CreateMap<AddStockCenterCommand, AddStockCenterResponse>().ReverseMap();
            CreateMap<AddStockCenterResponse, StockCenter>().ReverseMap();
            CreateMap<UpdateStockCenterCommand, StockCenter>().ReverseMap();
            CreateMap<GetStockQueryByIdResponse, StockCenter>().ReverseMap();
            #endregion

            #region StockAdjustment

            
            CreateMap<StockAdjustment, UpdateStockAdjustmentResponse>().ReverseMap();
            CreateMap<AddStockAdjustmentCommand, StockAdjustment>();
            CreateMap<StockAdjustment, AddStockAdjustmentResponse>();
            CreateMap<StockAdjustment, GetAllStockAdjustmentQueryResponse>().ReverseMap();
            CreateMap<PagedResult<StockAdjustment>, PagedResult<GetAllStockAdjustmentQueryResponse>>().ReverseMap();
            CreateMap<UpdateStockAdjustmentCommand, StockAdjustment>().ReverseMap();
            
            
            
            #endregion

            #region StockTransfer

            CreateMap<StockTransferDetails, GetAllStockTransferDetailsQueryResponse>().ReverseMap();
            CreateMap<PagedResult<StockTransferDetails>,PagedResult<GetAllStockTransferDetailsQueryResponse>>().ReverseMap();
            CreateMap<GetStockTransferDetailsByIdQueryResponse, StockTransferDetails>().ReverseMap();
            CreateMap<StockTransferDetails, UpdateStockTransferDetailsResponse>()
        .ForMember(dest => dest.addStockTransferItemsRequests,
                   opt => opt.MapFrom(src => src.StockTransferItems));

            CreateMap<StockTransferItems, AddStockTransferItemsRequest>();
            #endregion
        }
    }
}