using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using idp.Models;
using idp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace idp.Controllers
{
    [Produces("application/json")]
    [Route("api/callback")]
    public class CallbackController : Controller
    {
        private NDIDService _ndid;
        private ILogger _logger;
        private IPersistanceStorageService _db;

        public CallbackController(NDIDService ndid, ILogger<CallbackController> logger, IPersistanceStorageService db)
        {
            _ndid = ndid;
            _logger = logger;
            _db = db;
        }

        [HttpPost]
        [Route("accessor")]
        public async Task<IActionResult> AccessorSign([FromBody] AccessorSignRequestModel request)
        {
            // retrive 
            string signedString = await _ndid.AccessorSign(request.ReferenceId, request.SIdHash);
            AccessorSignResponseModel response = new AccessorSignResponseModel();
            response.Signature = signedString;
            return Ok(response);
        }

        [HttpPost]
        [Route("identity")]
        public async Task<IActionResult> IdentityResult([FromBody] NDIDCallbackRequestModel request)
        {
            if (request.Type == NDIDConstant.CallbackType.ADD_IDENTITY_REQUEST_RESULT)
            {
                await _ndid.HandleCreateIdentityRequestCallback(request);
            }
            else if (request.Type == NDIDConstant.CallbackType.ADD_IDENTITY_RESULT)
            {
                await _ndid.HandleCreateIdentityResultCallback(request);
            }
            else throw new NotImplementedException();
            return NoContent();
        }
    }
}