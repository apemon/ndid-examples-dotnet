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

        public CallbackController(INDIDService ndid, ILogger<CallbackController> logger)
        {
            _ndid = ndid;
            _logger = logger;
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
        public Task<IActionResult> AccessorSign([FromBody] AccessorSignRequestModel request)
        {
            _ndid.AccessorSign("hello", "hahaha");
            throw new NotImplementedException();
        }


    }
}