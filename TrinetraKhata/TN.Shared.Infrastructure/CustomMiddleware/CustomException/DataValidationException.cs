using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Infrastructure.CustomMiddleware.CustomException
{
    public class DataValidationException : Exception
    {
        public DataValidationException()
        {
            
        }
        public DataValidationException(
            string message
            ) : base(message)
        {
            
        }

        public System.Net.HttpStatusCode StatusCode = System.Net.HttpStatusCode.BadRequest;
    }
}
