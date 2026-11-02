using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.ServiceInterface
{
    public interface IDateConvertHelper
    {
        Task<DateTime> ConvertToEnglish(string nepaliDate);
        Task<string> ConvertToNepali(DateTime englishDate);
    }
}
