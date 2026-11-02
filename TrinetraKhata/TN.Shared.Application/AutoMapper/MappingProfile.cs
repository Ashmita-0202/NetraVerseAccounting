using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Shared.Application.Shared.Command.CloseFiscalYear;
using TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear;
using TN.Shared.Application.Shared.Command.UpdateExpiredDateItemStatusByCompany;
using TN.Shared.Application.Shared.Command.UpdateInventoryMethodByCompany;
using TN.Shared.Application.Shared.Command.UpdateItemStatusByCompany;
using TN.Shared.Application.Shared.Command.UpdateJournalRefByCompany;
using TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdatePurchaseRefNumberByCompany;
using TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType;
using TN.Shared.Application.Shared.Command.UpdateSalesQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberByCompany;
using TN.Shared.Application.Shared.Command.UpdateSalesReturnType;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInPurchase;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInSales;
using TN.Shared.Application.Shared.Queries.GetAllCurrentFiscalYear;
using TN.Shared.Application.Shared.Queries.GetPurchaseQuotationNumber;
using TN.Shared.Application.Shared.Queries.GetPurchaseReturnNumber;
using TN.Shared.Application.Shared.Queries.GetSalesQuotationNumberType;
using TN.Shared.Application.Shared.Queries.GetSalesReturnNumber;
using TN.Shared.Application.Shared.Queries.GetSelectedFiscalYear;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;


namespace TN.Shared.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region Inventory Settings
            CreateMap<CloseFiscalYearCommand, CloseFiscalYearResponse>().ReverseMap();
            CreateMap<UpdateFiscalYearResponse,UpdateFiscalYearCommand>().ReverseMap();
            CreateMap<GetAllFiscalYearQueryResponse, FiscalYears>().ReverseMap();
            CreateMap<PagedResult<FiscalYears>, PagedResult<GetAllFiscalYearQueryResponse>>().ReverseMap();
            CreateMap<UpdateItemStatusByCompanyResponse, UpdateItemStatusByCompanyCommand>().ReverseMap();
            //CreateMap<GetJournalRefByCompanyQueryResponse, CompanySettings>().ReverseMap();
            CreateMap<UpdateJournalRefByCompanyResponse, UpdateJournalRefByCompanyCommand>().ReverseMap();
            CreateMap<UpdateSalesReferenceNumberResponse, UpdateSalesReferenceNumberCommand>().ReverseMap();
           CreateMap<UpdateInventoryMethodResponse, UpdateInventoryMethodCommand>().ReverseMap();
            CreateMap<UpdatePurchaseReferenceNumberResponse, UpdatePurchaseReferenceNumberCommand>().ReverseMap();
           CreateMap<UpdateTaxStatusInPurchaseResponse, UpdateTaxStatusInPurchaseCommand>().ReverseMap();
            CreateMap<UpdateTaxStatusInSalesResponse, UpdateTaxStatusInSalesCommand>().ReverseMap();
            #endregion


            CreateMap<GetSelectedFiscalYearQueryResponse, FiscalYears>().ReverseMap();
            CreateMap<PagedResult<FiscalYears>, PagedResult<GetSelectedFiscalYearQueryResponse>>().ReverseMap();

            CreateMap<GetPurchaseReturnNumberQueryResponse,CompanySettings>().ReverseMap();
            CreateMap<GetSalesReturnNumberQueryResponse, CompanySettings>().ReverseMap();
          
            CreateMap<UpdatePurchaseReturnTypeResponse, UpdatePurchaseReturnTypeCommand>().ReverseMap();
           CreateMap<UpdateSalesReturnTypeResponse, UpdateSalesReturnTypeCommand>().ReverseMap();
            
            CreateMap<GetPurchaseQuotationNumberQueryResponse, CompanySettings>().ReverseMap();
            CreateMap<GetSalesQuotationTypeQueryResponse, CompanySettings>().ReverseMap();
       
            CreateMap<UpdatePurchaseQuotationTypeResponse, UpdatePurchaseQuotationTypeCommand>().ReverseMap();
            CreateMap<UpdateSalesQuotationTypeResponse, UpdateSalesQuotationTypeCommand>().ReverseMap();
        }
    }
}   
