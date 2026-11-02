using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.Static.Cache
{
    public static class LedgerGroupConstrants
    {


        public const string DirectExpenseLedgerId = "f6d7e8b9-a012-3456-f789-g012h345i678";
        public const string DirectIncomeLedgerId = "x4r5s6t7-a890-1234-x567-y890z123a456";
        //public const string CurrentAssets = "a1d2f3b4-c5e6-7890-a123-b456c789d012";
        //public const string CurrentLiabilities = "e5c6d7f8-a901-2345-e678-f901g234h567";
        //public const string FixedAssets = "d4b5c6f7-a890-1234-d567-e890f123g456";
        //public const string ShareHolderEquity = "y5s6t7u8-a901-2345-y678-z901a234b567";
        public const string IndirectIncomeLedgerId = "n4h5i6j7-a890-1234-n567-o890p123q456";
        public const string IndirectExpenseLedgerId = "g7e8f9c0-a123-4567-g890-h123i456j789";

        // Assets
        public const string CurrentAssets = "a1d2f3b4-c5e6-7890-a123-b456c789d012";
        public const string FixedAssets = "d4b5c6f7-a890-1234-d567-e890f123g456";
                                                                      
        // Liabilities
        public const string CurrentLiabilities = "e5c6d7f8-a901-2345-e678-f901g234h567"; // Current Liabilities
        public const string LongTermLiabilities = "j0d1e2f3-a456-7890-j123-k456l789m012"; // Long Term Liabilities
        public const string LoansLiability = "aed6b705-11aa-4b87-8681-49615d576a6d";      // Loans (Liability)
        public const string ShareHolderEquity = "y5s6t7u8-a901-2345-y678-z901a234b567";    // Share Holder's Equity

        // Duties & Taxes
        public const string DutiesAndTaxesLedgerId = "e84cfc6a-2289-4b4a-9ec3-88dc7f0bdf34"; // Duties & Taxes SubLedger
        public const string RoundOffLedgerId = "204b702f-12b5-43f8-a8eb-b3cb6e4ca60a"; // RoundOff A/C
        public const string TaxLedgerId = "5fa06b9c-ba4f-4d37-b049-a2e742bfc8d3"; // Tax A/C
        public const string TDSLedgerId = "da288c41-f087-41b4-b9ee-ebd2098ca4a9"; // TDS A/C
        public const string SalaryTDSLedgerId = "5ed3243e-f7f1-4d91-b39f-24da38d65c0b"; // Salary TDS A/C
    }
}


