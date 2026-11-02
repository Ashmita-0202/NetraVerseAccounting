using NepaliCalendarBS;
using NepaliDateConverter.Net;
using System;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;

namespace TN.Shared.Infrastructure.Repository
{
    public class DateConvertHelper : IDateConvertHelper
    {
        public Task<DateTime> ConvertToEnglish(string nepaliDate)
        {
            try
            {

                string[] nepaliNumbers = { "०", "१", "२", "३", "४", "५", "६", "७", "८", "९" };
                for (int i = 0; i < nepaliNumbers.Length; i++)
                {
                    nepaliDate = nepaliDate.Replace(nepaliNumbers[i], i.ToString());
                }

                DateTime englishDate = NepaliCalendarBS.NepaliCalendar.Convert_BS2AD(nepaliDate);
                return Task.FromResult(englishDate);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error converting Nepali date to English", ex);
            }
        }

        public Task<string> ConvertToNepali(DateTime englishDate)
        {
            try
            {
                // Convert English (AD) date to Nepali (BS)
                var nepaliDate = NepaliCalendarBS.NepaliCalendar.Convert_AD2BS(englishDate).ToString();

                // Convert digits to Nepali numerals
                //string[] nepaliNumbers = { "०", "१", "२", "३", "४", "५", "६", "७", "८", "९" };
                nepaliDate = nepaliDate.Replace("/", "-");

                //for (int i = 0; i < nepaliNumbers.Length; i++)
                //{
                //    nepaliDate = nepaliDate.Replace(i.ToString(), nepaliNumbers[i]);
                //}

                return Task.FromResult(nepaliDate);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error converting English date to Nepali", ex);
            }
        }
    }
}
