using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using TN.Setup.Application.Setup.Command.AddInstitution;
using TN.Setup.Domain.Entities;

namespace TN.Setup.Application.Setup.Command.AddCompany.RequestCommandMapper
{
    public static class AddCompanyRequestMapper
    {
        public static AddCompanyCommand ToCommand(this AddCompanyRequest addCompanyRequest)
        {
          return new AddCompanyCommand
                     (

                       addCompanyRequest.name,
                        addCompanyRequest.address,
                        addCompanyRequest.shortName,
                       addCompanyRequest.email,
                        addCompanyRequest.contactNumber,
                       addCompanyRequest.contactPerson,
                       addCompanyRequest.pan,
                       addCompanyRequest.imageUrl,
                        addCompanyRequest.isEnabled,
                       addCompanyRequest.institutionId,
                        addCompanyRequest.isDeleted,
                        addCompanyRequest.fiscalYearId,
                        addCompanyRequest.billNumberGenerationTypeForPurchase,
                        addCompanyRequest.billNumberGenerationTypeForSales
                        
                        

                      );


        }
    }
}
