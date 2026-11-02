using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.IRepository;

namespace TN.Authentication.Infrastructure.ServiceImpl
{
    public class OtpServices : IOtpServices
    {

        private readonly IConfiguration _configuration;
        private readonly IEmailServices _emailServices;
        private readonly Dictionary<string, int> _otpStore = new Dictionary<string, int>();

        public OtpServices(IConfiguration configuration, IEmailServices emailServices)
        {
            _configuration = configuration;
            _emailServices = emailServices;
            
        }
        public async Task<bool> SendOTPAsync(string email, int otp)
        {
            _otpStore[email] = otp;

            var data = _otpStore[email];
            var subject = "OTP Verification to Register";
            var body = $"Your OTP is:{otp}";
            await _emailServices.SendEmailAsync(email, subject, body);
            return true;
        }

        public Task<bool> VerifyOTPAsync(string email, int otp)
        {
            throw new NotImplementedException();
        }
    }
}
