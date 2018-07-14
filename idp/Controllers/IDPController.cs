using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using idp.Services;
using idp.Models;
using idp.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace idp.Controllers
{
    [Produces("application/json")]
    [Route("api/IDP")]
    public class IDPController : Controller
    {
        private INDIDService _ndid;
        private ILogger _logger;

        public IDPController(INDIDService ndid, ILogger<IDPController> logger)
        {
            _ndid = ndid;
            _logger = logger;
        }

        [HttpGet]
        [Route("requests/{namespaces}/{identifier}")]
        public IActionResult ListUserRequests([FromRoute] string namespaces, string identifier)
        {
            return Ok(_ndid.ListUserRequest(namespaces, identifier));
        }

        [HttpPost]
        [Route("identity")]
        public async Task<IActionResult> CreateNewIdentity([FromBody] IdentityRequest request)
        {
            NewIdentityModel iden = new NewIdentityModel();
            iden.NameSpace = request.NameSpace;
            iden.Identifier = request.Identifier;
            await _ndid.CreateNewIdentity(iden);
            return NoContent();
        }

        [HttpPost]
        [Route("callback")]
        public async Task<IActionResult> UpdateCallbackUrl([FromBody] NDIDGetCallbackModel model)
        {
            await _ndid.SetCallback(model);
            return NoContent();
        }


        [HttpPost]
        [Route("accept")]
        public async Task<IActionResult> AcceptAuthentication(AuthenticationRequest request)
        {
            await _ndid.CreateIDPResponse(request.Namespace, request.Identifier, request.RequestId, "accept");
            return Accepted();
        }

        [HttpPost]
        [Route("reject")]
        public async Task<IActionResult> RejectAuthentication(AuthenticationRequest request)
        {
            await _ndid.CreateIDPResponse(request.Namespace, request.Identifier, request.RequestId, "reject");
            return Accepted();
        }
    }
}