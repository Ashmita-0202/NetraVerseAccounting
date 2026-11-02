using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.GetCompanyDetailsByCompanyId
{
    public record  GetCompanyDetailsByCompanyIdQueryResponse
    (
            string id="",
            string name ="",
            string address = "",
            string pan = "",
            string phoneNumber = "",
             int   totalPurchaseBills = default,
             int   totalSalesBills = default,
            decimal totalPurchaseAmount = default,
            decimal totalSalesAmount = default,
            decimal totalVatPurchase = default,
            decimal totalVatSales = default


    );
}
