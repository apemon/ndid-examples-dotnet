using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using idp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace idp.Controllers
{
    [Produces("application/json")]
    [Route("api/callback")]
    public class CallbackController : Controller
    {
        private INDIDService _ndid;

        public CallbackController(INDIDService ndid)
        {
            _ndid = ndid;
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetCallback()
        {
            return Ok(await _ndid.GetCallback());
        }

        [HttpPost]
        [Route("accessor")]
        public Task<IActionResult> AccessorSign()
        {
            _ndid.AccessorSign("hello", "hahaha");
            throw new NotImplementedException();
        }


    }
}