using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Infrastructure.CustomMiddleware.CustomException
{
    public class DuplicateRecordException : Exception
    {
        public DuplicateRecordException()
        {
            
        }

        public DuplicateRecordException(string message) : base(message)
        {
            
        }

        public System.Net.HttpStatusCode StatusCode { get; } = System.Net.HttpStatusCode.Conflict;
    }
}
