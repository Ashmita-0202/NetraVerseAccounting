
using Microsoft.AspNetCore.Http;
using TN.receiptDatas.Application.receiptDatas.Command.UpdateReceipt;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Transactions.Application.Transactions.Command.AddReceipt;
using TN.Transactions.Application.Transactions.Command.ImportExcelForReceipt;
using TN.Transactions.Application.Transactions.Queries.FilterReceiptByDate;
using TN.Transactions.Application.Transactions.Queries.GetAllReceipt;
using TN.Transactions.Application.Transactions.Queries.GetReceiptById;

namespace TN.Transactions.Application.ServiceInterface
{
    public interface IReceiptServices
    {
        Task<Result<AddReceiptResponse>> Add(AddReceiptCommand addReceiptCommand);
        Task<Result<PagedResult<GetAllReceiptQueryResponse>>> GetAll(PaginationRequest paginationRequest,string? ledgerId,CancellationToken cancellationToken=default);

        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<GetReceiptByIdQueryResponse>> GetReceiptById(string id, CancellationToken cancellationToken=default);
         Task<Result<UpdateReceiptResponse>> Update(UpdateReceiptCommand updateReceiptCommand, string id);
    
        Task<Result<PagedResult<GetFilterReceiptQueryRespopnse>>> GetFilterReceipt(PaginationRequest paginationRequest,FilterReceiptDto filterReceiptDto);

        Task<Result<ReceiptExcelResponse>> AddReceiptExcel(IFormFile formFile, CancellationToken cancellationToken = default);


    }
}
