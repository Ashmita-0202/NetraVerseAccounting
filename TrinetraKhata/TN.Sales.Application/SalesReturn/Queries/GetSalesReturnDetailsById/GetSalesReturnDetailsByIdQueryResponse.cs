using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.SalesReturn.Queries.GetSalesReturnDetailsById
{
    public record  GetSalesReturnDetailsByIdQueryResponse
    (
            string id="",
            string salesDetailsId = "",
            string returnDate= "",
            decimal totalReturnAmount=0,
            decimal taxAdjustment = 0,
            decimal netReturnAmount = 0,
            string reason = "",
            string createdBy = "",
            DateTime createdAt = default,
            string modifiedBy = "",
            DateTime modifiedAt = default,
            string? StockCenterId=null,
                                    string? chequeNumber= null,
string? bankName = null,
string? accountName = null

    );
}
