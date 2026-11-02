using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TN.receiptDatas.Application.receiptDatas.Command.UpdateReceipt;
using TN.Shared.Domain.Entities.Transactions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Transactions.Application.Transactions.Command.AddExpense;
using TN.Transactions.Application.Transactions.Command.AddIncome;
using TN.Transactions.Application.Transactions.Command.AddPayments;
using TN.Transactions.Application.Transactions.Command.AddReceipt;
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using TN.Transactions.Application.Transactions.Command.AddTransactions;
using TN.Transactions.Application.Transactions.Command.UpdateExpense;
using TN.Transactions.Application.Transactions.Command.UpdateIncome;
using TN.Transactions.Application.Transactions.Command.UpdatePayment;
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using TN.Transactions.Application.Transactions.Queries.GetAllExpense;
using TN.Transactions.Application.Transactions.Queries.GetAllReceipt;
using TN.Transactions.Application.Transactions.Queries.GetAllTransactions;
using TN.Transactions.Application.Transactions.Queries.GetReceiptById;
using TN.Transactions.Application.Transactions.Queries.GetTransactionsById;

namespace TN.Transactions.Application.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            #region Transactions
            CreateMap<TransactionDetail, AddTransactionsResponse>()
                .ForMember(dest => dest.TransactionsItems, opt => opt.MapFrom(src => src.TransactionsItems))
                .ReverseMap();
            CreateMap<AddTransactionItemsRequest, TransactionDetail>().ReverseMap();
            CreateMap<AddTransactionItemsRequest, TransactionItems>().ReverseMap();
            CreateMap<GetAllTransactionsByQueryResponse, TransactionDetail>().ReverseMap();
            CreateMap<PagedResult<TransactionDetail>, PagedResult<GetAllTransactionsByQueryResponse>>().ReverseMap();
            CreateMap<GetTransactionsByIdQueryResponse, TransactionDetail>().ReverseMap();


            #endregion

            #region Receipt
            CreateMap<TransactionDetail, AddReceiptResponse>()
           .ForMember(dest => dest.TransactionsItems, opt => opt.MapFrom(src => src.TransactionsItems)).ReverseMap();

            CreateMap<GetAllReceiptQueryResponse, TransactionDetail>().ReverseMap();
            CreateMap<PagedResult<TransactionDetail>, PagedResult<GetAllReceiptQueryResponse>>().ReverseMap();
            CreateMap<GetReceiptByIdQueryResponse, TransactionDetail>().ReverseMap();

            CreateMap<UpdateReceiptCommand, TransactionDetail>();
            CreateMap<TransactionDetail, UpdateReceiptResponse>();
            CreateMap<UpdateTransactionItemRequest, TransactionItems>().ReverseMap();

            #endregion

            #region Income
            CreateMap<TransactionDetail, AddIncomeResponse>()
           .ForMember(dest => dest.TransactionsItems, opt => opt.MapFrom(src => src.TransactionsItems)).ReverseMap();
            CreateMap<AddPaymentsResponse, TransactionDetail>().ReverseMap();
            CreateMap<UpdateIncomeCommand, TransactionDetail>();
            CreateMap<TransactionDetail, UpdateIncomeResponse>();
            #endregion

            #region Payments
            CreateMap<TransactionDetail, AddPaymentsResponse>()
       .ForMember(dest => dest.TransactionsItems, opt => opt.MapFrom(src => src.TransactionsItems)).ReverseMap();
            CreateMap<UpdatePaymentCommand, TransactionDetail>();
            
            #endregion


            #region Expense
            CreateMap<TransactionDetail, AddExpenseResponse>()
       .ForMember(dest => dest.TransactionsItems, opt => opt.MapFrom(src => src.TransactionsItems)).ReverseMap();
            CreateMap<UpdateExpenseCommand, TransactionDetail>();
            CreateMap<GetAllExpenseQueryResponse, TransactionDetail>().ReverseMap();
            CreateMap<PagedResult<TransactionDetail>, PagedResult<GetAllExpenseQueryResponse>>().ReverseMap();
            #endregion

        }
    }
}
