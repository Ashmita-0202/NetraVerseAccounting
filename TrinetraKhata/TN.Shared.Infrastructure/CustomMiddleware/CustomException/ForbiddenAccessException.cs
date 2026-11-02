using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Infrastructure.CustomMiddleware.CustomException
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException()
        {
        }
        public ForbiddenAccessException(string message) : base(message)
        {
        }
        //Define the property of the status code
        public System.Net.HttpStatusCode StatusCode { get; } = System.Net.HttpStatusCode.Forbidden;
    }
}
