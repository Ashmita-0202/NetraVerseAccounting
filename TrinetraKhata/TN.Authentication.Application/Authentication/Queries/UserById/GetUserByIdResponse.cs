using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Queries.UserById
{
    public record GetUserByIdResponse
    (
      string Id,
      string UserName,
      string Address,
      string FirstName,
      string LastName,
      string Email

    );
}
