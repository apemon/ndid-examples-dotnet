using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace idp.Controllers
{
    [Produces("application/json")]
    [Route("api/callback")]
    public class CallbackController : Controller
    {
        [HttpPost]
        [Route("accessor")]
        public Task<IActionResult> AccessorSign()
        {
            throw new NotImplementedException();
        }
    }
}