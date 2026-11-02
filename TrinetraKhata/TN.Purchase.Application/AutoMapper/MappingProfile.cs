using AutoMapper;
using TN.Purchase.Application.Purchase.Command.AddPurchaseDetails;
using TN.Purchase.Application.Purchase.Command.AddPurchaseItems;
using TN.Purchase.Application.Purchase.Command.DeletePurchaseDetails;
using TN.Purchase.Application.Purchase.Command.UpdatePurchaseDetails;
using TN.Purchase.Application.Purchase.Queries.GetAllPurchaseItems;
using TN.Purchase.Application.Purchase.Queries.GetPurchaseDetailsByRefNo;
using TN.Purchase.Application.Purchase.Queries.GetPurchaseQuotationById;
using TN.Purchase.Application.Purchase.Queries.Purchase;
using TN.Purchase.Application.Purchase.Queries.PurchaseDetailsById;
using TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnDetails;
using TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnItems;

using TN.Purchase.Application.PurchaseReturn.Command.UpdatePurchaseReturnDetails;
using TN.Purchase.Application.PurchaseReturn.Queries.AllPurchaseReturnDetails;
using TN.Purchase.Domain.Entities;
using TN.Sales.Domain.Entities;
using TN.Shared.Domain.Entities.Purchase;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using static TN.Authentication.Domain.Entities.Company;

public class PurchaseMappingProfile : Profile
{
    public PurchaseMappingProfile()
    {
        #region PurchaseQuotationDetails
        CreateMap<PurchaseQuotationDetails, AddPurchaseDetailsResponse>().ReverseMap();
            CreateMap<GetPurchaseQuotationByIdQueryResponse,PurchaseQuotationDetails>().ReverseMap();
        #endregion

        #region Purchase Details
        CreateMap<AddPurchaseDetailsCommand, PurchaseDetails>();
        CreateMap<AddPurchaseDetailsResponse, PurchaseDetails>().ReverseMap();
        CreateMap<GetAllPurchaseDetailsQueryResponse, PurchaseDetails>().ReverseMap();
        CreateMap<PagedResult<PurchaseDetails>, PagedResult<GetAllPurchaseDetailsQueryResponse>>().ReverseMap();
        CreateMap<GetPurchaseDetailsByIdQueryResponse, PurchaseDetails>().ReverseMap();
        CreateMap<PurchaseDetails, DeletePurchaseDetailsCommand>().ReverseMap();
        CreateMap<UpdatePurchaseDetailsCommand, PurchaseDetails>().ReverseMap();
       CreateMap<UpdatePurchaseItems, PurchaseItems>().ReverseMap();
        CreateMap<GetPurchaseDetailsQueryResponse, PurchaseDetails>().ReverseMap();
        CreateMap<PurchaseItems, QuantityDetailDto>()
           .ForMember(dest => dest.serialNumbers, opt => opt.MapFrom(src =>
               src.ItemInstances.Where(s => !string.IsNullOrWhiteSpace(s.SerialNumber))
                                .Select(s => s.SerialNumber).ToList()
           ));
        #endregion

        CreateMap<UpdatePurchaseItemsDTOs, PurchaseItems>()
    .ForMember(dest => dest.ItemInstances, opt => opt.Ignore());


        #region Purchase Items
        CreateMap<PurchaseItems, AddPurchaseItemsRequest>();
        CreateMap<PurchaseItems, UpdatePurchaseItemsDTOs>().ReverseMap();
        CreateMap<PurchaseItems, UpdatePurchaseDetailsCommand>().ReverseMap();
        CreateMap<AddPurchaseItemsResponse, PurchaseItems>().ReverseMap();
        CreateMap<GetAllPurchaseItemsByQueryResponse, PurchaseItems>().ReverseMap();
        CreateMap<PagedResult<PurchaseItems>, PagedResult<GetAllPurchaseItemsByQueryResponse>>().ReverseMap();
        #endregion


        #region PurchaseReturnDetails
        CreateMap<AddPurchaseReturnDetailsCommand, PurchaseReturnDetails>();
        CreateMap<AddPurchaseReturnDetailsResponse, PurchaseReturnDetails>().ReverseMap();
        CreateMap<PurchaseReturnDetailsQueryResponse, PurchaseReturnDetails>().ReverseMap();
        CreateMap<PagedResult<PurchaseReturnDetails>, PagedResult<PurchaseReturnDetailsQueryResponse>>().ReverseMap();
        CreateMap<AddPurchaseReturnDetailsResponse, PurchaseReturnDetails>().ReverseMap();
        CreateMap<UpdatePurchaseReturnDetailsCommand, PurchaseReturnDetails>().ReverseMap();
        CreateMap<UpdatePurchaseReturnItems, PurchaseReturnItems>().ReverseMap();

        #endregion
        CreateMap<AddPurchaseReturnItemsRequest, PurchaseReturnItems>().ReverseMap();
        #region PurchaseReturnItems


        #endregion
    }
}
