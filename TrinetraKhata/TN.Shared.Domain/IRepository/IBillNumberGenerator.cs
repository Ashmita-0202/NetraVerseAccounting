using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Authentication.Domain.Entities.CompanySettings;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Shared.Domain.IRepository
{
    public interface IBillNumberGenerator
    {
        Task<string> GenerateBillNumberAsync(string companyId, string billType, string fyName);

        Task<string> GenerateTransactionNumber(string companyId, string transactionNumberType, string fyName);
        Task<string> GenerateJournalReference(string companyId);

        Task<string> GenerateReferenceNumber(string companyId, ReferenceType referenceType);
    }
}
