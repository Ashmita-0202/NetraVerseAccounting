using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface.BackGroundServices;
using TN.Shared.Domain.Entities.Notification;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.Data;
using static System.Formats.Asn1.AsnWriter;

namespace TN.Shared.Infrastructure.CustomMiddleware.FiscalYearContext
{
    public class FiscalContextMiddleware
    {

        private readonly RequestDelegate _next;


        public FiscalContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context, FiscalContext fiscalContext, IFiscalYearService fiscalYearService ,ITokenService tokenService )
        {

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();


  

                if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                await _next(context);
                return;
            }
            var companyId = tokenService.CompanyId().FirstOrDefault();

            if (!string.IsNullOrEmpty(companyId))
            {
                var fiscalYear = await fiscalYearService.GetCurrentFiscalYearFromSettingsAsync();
                fiscalContext.CurrentCompanyId = companyId;
                fiscalContext.CurrentFiscalYearId = fiscalYear.Id;
            }

            await _next(context);

        }
    }
}
