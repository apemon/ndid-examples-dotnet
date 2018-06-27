using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using idp.Adapters;
using idp.Models;
using idp.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace idp.Controllers
{
    [Produces("application/json")]
    [Route("api/IDP")]
    public class IDPController : Controller
    {
        private INDIDAdapter _ndid;

        public IDPController()
        {
            // dependency injecton
            _ndid = new NDIDAdapter();    
        }

        /*
        public IDPController(INDIDAdapter ndid, IDPKIAdapter dpki)
        {
            if (ndid == null) _ndid = new NDIDAdapter();
            else _ndid = ndid;
            if (dpki == null) _dpki = new FileBasedDPKIAdapter();
            else _dpki = dpki;
        }
        */

        [HttpPost]
        [Route("identity")]
        public async Task<IActionResult> CreateNewIdentity(IdentityRequest request)
        {
            NewIdentityModel iden = new NewIdentityModel();
            iden.NameSpace = request.NameSpace;
            iden.Identifier = request.Identifier;
            await _ndid.CreateNewIdentity(iden);
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