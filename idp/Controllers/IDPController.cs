using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using idp.Adapters;
using idp.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace idp.Controllers
{
    [Produces("application/json")]
    [Route("api/IDP")]
    public class IDPController : Controller
    {
        INDIDAdapter ndid;
        IDPKIAdapter dpki;

        public IDPController()
        {
            // dependency injecton
        }

        [HttpPost]
        [Route("identity")]
        public async Task<IActionResult> CreateNewIdentity(IdentityRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("accessors")]
        public async Task<IActionResult> AddNewAccessor()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("accept")]
        public async Task<IActionResult> AcceptAuthentication(AuthenticationRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("reject")]
        public async Task<IActionResult> RejectAuthentication(AuthenticationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}