using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NV.Payment.Application.Payment.Command.AddPayment;
using NV.Payment.Application.Payment.Command.UpdatePayment;
using NV.Payment.Application.Payment.Queries.FilterPaymentMethod;
using NV.Payment.Application.Payment.Queries.GetPaymentMethod;
using NV.Payment.Application.Payment.Queries.GetPaymentMethodById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace NV.Payment.Application.ServiceInterface
{
    public interface IPaymentMethodService
    {
        Task<Result<AddPaymentMethodResponse>> Add(AddPaymentMethodCommand command);
        Task<Result<PagedResult<GetAllPaymentMethodQueryResponse>>> GetAllPaymentMethod(PaginationRequest paginationRequest,CancellationToken cancellationToken=default);
        Task<Result<GetPaymentMethodByIdQueryResponse>> GetPaymentMethodById(string id, CancellationToken cancellationToken=default);
        Task<Result<UpdatePaymentMethodResponse>> Update(string id,UpdatePaymentMethodCommand command);
        Task<Result<bool>> Delete(string id,CancellationToken cancellationToken);
        Task<Result<PagedResult<GetFilterPaymentMethodResponse>>> GetPaymentMethodFilter(PaginationRequest paginationRequest, FilterPaymentMethodDto filterPaymentMethodDto);
    
    }
}
