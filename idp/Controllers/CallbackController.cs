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
        private INDIDService _ndid;
        private ILogger _logger;
        private IPersistanceStorageService _db;

        public CallbackController(INDIDService ndid, ILogger<CallbackController> logger, IPersistanceStorageService db)
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


    }
}