using idp.Responses;
using idp.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace idp.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfigurationService _config;

        public ExceptionHandlingMiddleware(RequestDelegate next, IConfigurationService config)
        {
            this._next = next;
            this._config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // handle exception
            var code = HttpStatusCode.InternalServerError;
            BaseResponse response = new BaseResponse();
            response.ErrorCode = "999";
            if(this._config.GetEnvironment() == "Development")
            {
                response.ErrorDescription = JsonConvert.SerializeObject(ex);
            } else
            {
                response.ErrorDescription = ex.Message;
            }
            string result = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(result);
        }
    }
}
