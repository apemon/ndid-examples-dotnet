using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idp.Exceptions
{
    public class APIInvalidException: ApplicationException
    {
        public string URL { get; set; }
        public string StatusCode { get; set; }
        public string Content { get; set; }

        public APIInvalidException()
        {

        }
        public APIInvalidException(string message)
            : base(message)
        {

        }

        public APIInvalidException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
