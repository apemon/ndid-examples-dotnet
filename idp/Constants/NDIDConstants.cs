using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idp.Services
{
    public class NDIDConstant
    {
        public class CallbackType
        {
            public const string ADD_IDENTITY_REQUEST_RESULT = "create_identity_request_result";
            public const string ADD_IDENTITY_RESULT = "create_identity_result";
            public const string INCOMING_REQUEST = "incoming_request";
            public const string RESPONSE_RESULT = "response_result";
        }
    }
}
