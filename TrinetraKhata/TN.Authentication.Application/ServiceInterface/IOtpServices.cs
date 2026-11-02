using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.ServiceInterface
{
    public interface IOtpServices
    {
        Task<bool> SendOTPAsync(string email, int otp);
        Task<bool> VerifyOTPAsync(string email, int otp);
    }
}
