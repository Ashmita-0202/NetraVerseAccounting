using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.InstitutionById
{
  public record GetInstitutionByIdResponse
  (
            string id,
            string name,
            string address,
            string email,
            string shortName,
            string contactNumber,
            string contactPerson,
            string pan,
            string imageUrl,
            DateTime createdDate,
            string createdBy,
            DateTime modifiedDate,
            string modifiedBy,
            bool isDeleted,
            string organizationId
  );
}
