using AutoMapper;
using TN.Account.Application.Account.Command.AddCustomer;
using TN.Account.Application.Account.Command.AddCustomerCategory;
using TN.Account.Application.Account.Command.AddJournalEntry;
using TN.Account.Application.Account.Command.AddJournalEntryDetails;
using TN.Account.Application.Account.Command.AddLedger;
using TN.Account.Application.Account.Command.AddLedgerGroup;
using TN.Account.Application.Account.Command.AddSubledgerGroup;
using TN.Account.Application.Account.Command.BillSundry;
using TN.Account.Application.Account.Command.DeleteCustomer;
using TN.Account.Application.Account.Command.DeleteCustomerCategory;
using TN.Account.Application.Account.Command.DeleteJournalEntry;
using TN.Account.Application.Account.Command.DeleteLedger;
using TN.Account.Application.Account.Command.DeleteLedgerGroup;
using TN.Account.Application.Account.Command.DeleteMaster;
using TN.Account.Application.Account.Command.UpdateBillSundry;
using TN.Account.Application.Account.Command.UpdateCustomer;
using TN.Account.Application.Account.Command.UpdateCustomerCategory;
using TN.Account.Application.Account.Command.UpdateJournalEntry;
using TN.Account.Application.Account.Command.UpdateJournalEntryDetails;
using TN.Account.Application.Account.Command.UpdateLedger;
using TN.Account.Application.Account.Command.UpdateLedgerGroup;
using TN.Account.Application.Account.Command.UpdateMaster;
using TN.Account.Application.Account.Command.UpdateSubledgerGroup;
using TN.Account.Application.Account.Queries.ChartOfAccounts;
using TN.Account.Application.Account.Queries.Customer;
using TN.Account.Application.Account.Queries.CustomerById;
using TN.Account.Application.Account.Queries.CustomerCategory;
using TN.Account.Application.Account.Queries.CustomerCategoryById;
using TN.Account.Application.Account.Queries.GetBalance;
using TN.Account.Application.Account.Queries.GetBillSundry;
using TN.Account.Application.Account.Queries.GetBillSundryById;
using TN.Account.Application.Account.Queries.GetLedgerGroupByMasterId;
using TN.Account.Application.Account.Queries.GetMasterById;
using TN.Account.Application.Account.Queries.JournalEntry;
using TN.Account.Application.Account.Queries.JournalEntryById;
using TN.Account.Application.Account.Queries.JournalEntryDetails;
using TN.Account.Application.Account.Queries.JournalEntryDetailsById;
using TN.Account.Application.Account.Queries.Ledger;
using TN.Account.Application.Account.Queries.LedgerById;
using TN.Account.Application.Account.Queries.LedgerByLedgerGroupId;
using TN.Account.Application.Account.Queries.LedgerGroup;
using TN.Account.Application.Account.Queries.LedgerGroupById;
using TN.Account.Application.Account.Queries.master;
using TN.Account.Application.Account.Queries.OpeningStockByCompanyId;
using TN.Account.Application.Account.Queries.Parties;
using TN.Account.Application.Account.Queries.SubledgerGroup;
using TN.Account.Application.Account.Queries.SubledgerGroupById;
using TN.Account.Domain.Entities;
using TN.Inventory.Domain.Entities;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.StockCenterEntities;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region BillSundry
            CreateMap<AddBillSundryResponse, BillSundry>().ReverseMap();
            CreateMap<GetBillSundryQueryResponse, BillSundry>().ReverseMap();
            CreateMap<PagedResult<BillSundry>, PagedResult<GetBillSundryQueryResponse>>().ReverseMap();
            CreateMap<GetBillSundryByIdQueryResponse, BillSundry>().ReverseMap();
            CreateMap<BillSundry,UpdateBillSundryCommand>().ReverseMap();


            #endregion

            #region AccountPayable
            CreateMap<GetBalanceByQueryResponse, JournalEntryDetails>().ReverseMap();
            CreateMap<PagedResult<JournalEntryDetails>, PagedResult<GetBalanceByQueryResponse>>().ReverseMap();
            #endregion
            #region Chart Of Accounting
            CreateMap<ChartsOfAccountsQueryResponse, Master>().ReverseMap();
            #endregion
            #region Master
            CreateMap<GetAllMasterByQueryResponse, Master>().ReverseMap();
            CreateMap<GetMasterByIdQueryResponse, Master>().ReverseMap();
            CreateMap<PagedResult<Master>, PagedResult<GetMasterByIdQueryResponse>>().ReverseMap();
            CreateMap<PagedResult<Master>, PagedResult<GetAllMasterByQueryResponse>>().ReverseMap();
            CreateMap<DeleteMasterCommand, Master>().ReverseMap();
            CreateMap<Master, UpdateMasterCommand>().ReverseMap();
            #endregion

            #region LedgerGroup
            CreateMap<GetAllLedgerGroupByQueryResponse, LedgerGroup>().ReverseMap();
            CreateMap<GetLedgerGroupByIdResponse, LedgerGroup>().ReverseMap();
            CreateMap<GetLedgerGroupByMasterIdResponse, LedgerGroup>().ReverseMap();
            CreateMap<PagedResult<LedgerGroup>, PagedResult<GetLedgerGroupByIdResponse>>().ReverseMap();
            CreateMap<PagedResult<LedgerGroup>, PagedResult<GetAllLedgerGroupByQueryResponse>>().ReverseMap();
            CreateMap<AddLedgerGroupResponse, LedgerGroup>().ReverseMap();
            CreateMap<AddLedgerGroupCommand, LedgerGroup>().ReverseMap();
            CreateMap<LedgerGroup, UpdateLedgerGroupCommand>().ReverseMap();
            CreateMap<LedgerGroup, DeleteLedgerGroupCommand>().ReverseMap();
            #endregion

            #region Ledger
            CreateMap<GetAllLedgerByQueryResponse, Ledger>().ReverseMap();
            CreateMap<GetLedgerByIdQueryResponse, Ledger>().ReverseMap();
            CreateMap<GetAllLedgerByLedgerGroupIdResponse, Ledger>().ReverseMap();
            CreateMap<PagedResult<Ledger>, PagedResult<GetLedgerByIdQueryResponse>>().ReverseMap();
            CreateMap<PagedResult<Ledger>, PagedResult<GetAllLedgerByQueryResponse>>().ReverseMap();
            CreateMap<AddLedgerResponse, Ledger>().ReverseMap();
            CreateMap<AddLedgerCommand, Ledger>().ReverseMap();
            CreateMap<AddLedgerCommand, AddLedgerResponse>().ReverseMap();
            CreateMap<Ledger, UpdateLedgerCommand>().ReverseMap();
            CreateMap<Ledger, DeleteLedgerCommand>().ReverseMap();
            #endregion

            #region SubledgerGroup
            CreateMap<AddSubledgerGroupResponse, SubLedgerGroup>().ReverseMap();
            CreateMap<AddSubledgerGroupCommand, SubLedgerGroup>().ReverseMap();
            CreateMap<SubLedgerGroup, GetAllSubledgerGroupQueryResposne>().ReverseMap();
            CreateMap<PagedResult<SubLedgerGroup>, PagedResult<GetAllSubledgerGroupQueryResposne>>().ReverseMap();
            CreateMap<GetSubledgerGroupByIdResponse, SubLedgerGroup>().ReverseMap();
            CreateMap<SubLedgerGroup, UpdateSubledgerGroupCommand>().ReverseMap();
            #endregion

            #region Parties
            CreateMap<GetAllPartiesByQueriesResponse, Ledger>().ReverseMap();
            CreateMap<PagedResult<Ledger>, PagedResult<GetAllPartiesByQueriesResponse>>().ReverseMap();
            #endregion

            #region Customer
            CreateMap<GetAllCustomerByQueryResponse, Customers>().ReverseMap();
            CreateMap<PagedResult<Customers>, PagedResult<GetAllCustomerByQueryResponse>>().ReverseMap();
            CreateMap<GetCustomerByIdResponse, Customers>().ReverseMap();
            CreateMap<AddCustomerResponse, Customers>().ReverseMap();
            CreateMap<AddCustomerCommand, Customers>().ReverseMap();
            CreateMap<Customers, DeleteCustomerCommand>().ReverseMap();
            CreateMap<Customers, UpdateCustomerCommand>().ReverseMap();
            #endregion

           

            #region CustomerCategory
            CreateMap<GetAllCustomerCategoryByResponse, CustomerCategory>().ReverseMap();
            CreateMap<PagedResult<CustomerCategory>, PagedResult<GetAllCustomerCategoryByResponse>>().ReverseMap();
            CreateMap<GetCustomerCategoryByIdResponse, CustomerCategory>().ReverseMap();
            CreateMap<CustomerCategory, DeleteCustomerCategoryCommand>().ReverseMap();
            CreateMap<AddCustomerCategoryResponse, CustomerCategory>().ReverseMap();
            CreateMap<AddCustomerCategoryCommand, CustomerCategory>().ReverseMap();
            CreateMap<CustomerCategory, UpdateCustomerCategoryCommand>().ReverseMap();
            #endregion

            #region Journal
            CreateMap<JournalEntry, AddJournalEntryResponse>()
                .ForMember(dest => dest.AddJournalEntryDetails, opt => opt.MapFrom(src => src.JournalEntryDetails))
                .ReverseMap();

            CreateMap<JournalEntryDetails, AddJournalEntryDetailsRequest>().ReverseMap();
            CreateMap<AddJournalEntryCommand, JournalEntry>().ReverseMap();
            CreateMap<JournalEntryDetails, AddJournalEntryDetailsResponse>().ReverseMap();
            CreateMap<JournalEntry, GetAllJournalEntryByQueryResponse>()
                .ForMember(dest => dest.journalEntries, opt => opt.MapFrom(src => src.JournalEntryDetails))
                .ReverseMap();
            CreateMap<GetJournalEntryByIdResponse, JournalEntry>().ReverseMap();
            CreateMap<DeleteJournalEntryCommand, JournalEntry>().ReverseMap();
            CreateMap<UpdateJournalEntryCommand, JournalEntry>().ReverseMap();
      

            
            #endregion

            #region JournalEntryDetails
            CreateMap<GetAllJournalEntryDetailsByQueryResponse, JournalEntryDetails>().ReverseMap();
            CreateMap<PagedResult<JournalEntryDetails>, PagedResult<GetAllJournalEntryDetailsByQueryResponse>>().ReverseMap();
            CreateMap<GetJournalEntryDetailsByIdResponse, JournalEntryDetails>().ReverseMap();
            CreateMap<UpdateJournalDetailsCommand, JournalEntryDetails>().ReverseMap();
            CreateMap<AddJournalEntryDetailsCommand, JournalEntryDetails>().ReverseMap();
            CreateMap<UpdateJournalEntryDetails, JournalEntryDetails>().ReverseMap();
            #endregion

            #region OpeningStock

          
            CreateMap<OpeningStockItemDto, Items>().ReverseMap();

            #endregion

        }
    }
}
