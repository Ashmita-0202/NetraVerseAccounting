using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.SalesReturn.Queries
{
    public record  GetAllSalesReturnDetailsByQueryResponse
    (
            string id,
            string salesDetailsId,
            string? returnDate,
            decimal totalReturnAmount,
            decimal? taxAdjustment,
            decimal? discount,
            decimal netReturnAmount,
            string reason,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            decimal totalQuantity,
            string ledgerId,
            string companyId,
            string? stockCenterId,
              decimal? taxableAmount,
             decimal? subTotalAmount,
            List<SalesReturnItemsDto> SalesReturnItems

    );
    public record SalesReturnItemsDto
      (
      string id,
          string salesReturnDetailsId,
          string salesItemsId,
          string itemId,
          decimal returnQuantity,
          decimal returnUnitPrice,
          decimal returnTotalPrice
      );
}
