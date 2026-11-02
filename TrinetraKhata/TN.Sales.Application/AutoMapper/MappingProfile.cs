using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.Sales.Command.AddSalesDetails;
using TN.Sales.Application.Sales.Command.AddSalesItems;
using TN.Sales.Application.Sales.Command.DeleteSalesDetails;
using TN.Sales.Application.Sales.Command.UpdateSalesDetails;
using TN.Sales.Application.Sales.Queries.AllSalesDetails;
using TN.Sales.Application.Sales.Queries.FilterSalesQuotationByDate;
using TN.Sales.Application.Sales.Queries.GetAllSalesItems;
using TN.Sales.Application.Sales.Queries.GetSalesQuotationById;
using TN.Sales.Application.Sales.Queries.SalesDetailsById;
using TN.Sales.Application.SalesReturn.Command.AddSalesReturnDetails;
using TN.Sales.Application.SalesReturn.Command.DeleteSalesReturnDetails;
using TN.Sales.Application.SalesReturn.Command.UpdateSalesReturnDetails;
using TN.Sales.Application.SalesReturn.Queries;
using TN.Sales.Application.SalesReturn.Queries.GetAllSalesReturnItems;
using TN.Sales.Application.SalesReturn.Queries.GetSalesReturnDetailsById;
using TN.Sales.Domain.Entities;
using TN.Shared.Domain.Entities.Sales;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Sales.Application.AutoMapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {

            #region SalesQuotation

            CreateMap<FilterSalesQuotationQueryResponse, SalesDetails>().ReverseMap();
            CreateMap<AddSalesItemsRequest, SalesQuotationItems>().ReverseMap();
            CreateMap<PagedResult<SalesDetails>, PagedResult<FilterSalesQuotationQueryResponse>>().ReverseMap();
            CreateMap<SalesQuotationDetails, AddSalesDetailsResponse>();
            CreateMap<GetSalesQuotationByIdQueryResponse, SalesQuotationDetails>().ReverseMap();
            #endregion
            #region Sales Details
            CreateMap<AddSalesDetailsCommand, SalesDetails>();
            CreateMap<AddSalesDetailsResponse, SalesDetails>().ReverseMap();
            CreateMap<UpdateSalesDetailsCommand, SalesDetails>().ReverseMap();
            CreateMap<UpdateSalesItems, SalesItems>().ReverseMap();
            CreateMap<GetAllSalesDetailsByQueryResponse, SalesDetails>().ReverseMap();
            CreateMap<PagedResult<SalesDetails>, PagedResult<GetAllSalesDetailsByQueryResponse>>().ReverseMap();
            CreateMap<GetSalesDetailsByIdQueryResponse, SalesDetails>().ReverseMap();
            CreateMap<SalesDetails, DeleteSalesDetailsCommand>().ReverseMap();
            CreateMap<GetSalesDetailsByIdQueryResponse, SalesDetails>().ReverseMap();
            CreateMap<SalesItems, QuantityDetailDto>()
            .ForMember(dest => dest.serialNumbers, opt => opt.MapFrom(src =>
                src.ItemInstances.Where(s => !string.IsNullOrWhiteSpace(s.SerialNumber))
                                 .Select(s => s.SerialNumber).ToList()
            ));


            #endregion

            #region Sales Items
            CreateMap<SalesItems, AddSalesItemsRequest>();
            CreateMap<SalesItems, UpdateSalesItemsDTOs>().ReverseMap();
            CreateMap<SalesItems, UpdateSalesDetailsCommand>().ReverseMap();
            CreateMap<SalesItems, SalesItemsDto>();
            CreateMap<GetAllSalesItemsByQueryResponse, SalesItems>().ReverseMap();
            CreateMap<PagedResult<SalesItems>, PagedResult<GetAllSalesItemsByQueryResponse>>().ReverseMap();
            #endregion

            #region SalesReturnDetails
            CreateMap<AddSalesReturnDetailsCommand, SalesReturnDetails>();
            CreateMap<AddSalesReturnDetailsResponse, SalesReturnDetails>().ReverseMap();
            CreateMap<GetAllSalesReturnDetailsByQueryResponse, SalesReturnDetails>().ReverseMap();
            CreateMap<PagedResult<SalesReturnDetails>, PagedResult<GetAllSalesReturnDetailsByQueryResponse>>().ReverseMap();
            CreateMap<GetSalesReturnDetailsByIdQueryResponse, SalesReturnDetails>().ReverseMap();
            CreateMap<UpdateSalesReturnDetailsCommand, SalesReturnDetails>().ReverseMap();
            CreateMap<SalesReturnDetails, DeleteSalesReturnDetailsCommand>().ReverseMap();
            #endregion

            #region SalesReturnItems
            CreateMap<GetAllSalesReturnItemsByQueryResponse, SalesReturnItems>().ReverseMap();
            CreateMap<PagedResult<SalesReturnItems>, PagedResult<GetAllSalesReturnItemsByQueryResponse>>().ReverseMap();


            #endregion

        }
    }
}
