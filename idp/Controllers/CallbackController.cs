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

        [HttpGet]
        [Route("retrieve/{key}")]
        public IActionResult GetDB([FromRoute] string key)
        {
            string result = _db.GetAccessorSign(key);
            return Ok(result);
        }

        [HttpPost]
        [Route("save")]
        public IActionResult SaveDB(AccessorSignRequestModel model)
        {
            _db.SaveAccessorSign(model.ReferenceId, model.SId);
            return NoContent();
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetCallback()
        {
            _logger.LogInformation("add identity");
            return Ok(await _ndid.GetCallback());
        }

        [HttpPost]
        [Route("accessor")]
        public async Task<IActionResult> AccessorSign([FromBody] AccessorSignRequestModel request)
        {
            // retrive 
            string signedString = await _ndid.AccessorSign(request.ReferenceId, request.SIdHash);
            return Ok(signedString);
        }


    }
}