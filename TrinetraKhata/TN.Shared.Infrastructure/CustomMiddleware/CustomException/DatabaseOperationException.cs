using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Infrastructure.CustomMiddleware.CustomException
{
    public class DatabaseOperationException : Exception
    {
        public DatabaseOperationException() { }

        public DatabaseOperationException(
            string message
            ): base(message)
        {
            
        }

        public System.Net.HttpStatusCode StatusCode { get; } = System.Net.HttpStatusCode.InternalServerError;
    }
}
