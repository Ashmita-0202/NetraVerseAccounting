using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Reports.Application.AccountBook.Queries.SalesRegister;
using TN.Reports.Application.Annex13.Queries;
using TN.Reports.Application.LedgerBalance.Queries.LedgerSummary;
using TN.Reports.Application.Parties_Statements.Queries;
using TN.Reports.Application.PurchaseReport;
using TN.Reports.Application.SalesReport;
using TN.Reports.Application.SalesReturn_Report;
using TN.Reports.Application.TradingAccount;
using TN.Sales.Domain.Entities;

namespace TN.Reports.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region AnnexReport
            //CreateMap<AnnexReportQueryResponse, SalesDetails>().ReverseMap();
            #endregion
            #region SalesRegister
            CreateMap<SalesRegisterQueryResponse, SalesDetails>().ReverseMap();
            #endregion

            #region LedgerBalance
            CreateMap<LedgerSummaryResponse, JournalEntryDetails>().ReverseMap();
            #endregion

            #region PurchaseReport 
            CreateMap<PurchaseDetails, GetPurchaseReportQueryResponse>()
           .ForMember(dest => dest.billNumber, opt => opt.MapFrom(src => src.BillNumber))
           .ForMember(dest => dest.ledgerId, opt => opt.MapFrom(src => src.LedgerId));
         

            CreateMap<PurchaseItems, GetPurchaseReportQueryResponse>()
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dest => dest.quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Price));

            CreateMap<PurchaseReturnItems, GetPurchaseReportQueryResponse>();
            CreateMap<PurchaseItems, PurchaseReportDtos>();

            #endregion

            #region SalesReport
            CreateMap<SalesItems, GetSalesReportQueryResponse>();
            CreateMap<SalesDetails, GetSalesReportQueryResponse>();
            CreateMap<SalesReportDtos, GetSalesReportQueryResponse>();
            #region
            CreateMap<SalesReturnItems,GetSalesReturnReportQueryResponse>();
            CreateMap<SalesReturnReportDto, GetSalesReturnReportQueryResponse>();
            CreateMap<SalesReturnDetails, GetSalesReturnReportQueryResponse>();
            #endregion
            #endregion

            #region TradingAccount
            CreateMap<JournalEntryDetails, GetTradingAccountQueryResponse>();
            CreateMap<JournalEntry, GetTradingAccountQueryResponse>();

            #endregion
            #region StockDetail Report
           
            #endregion

        }
    }
}
