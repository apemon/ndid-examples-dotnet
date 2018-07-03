using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idp.Middleware
{
    /// <summary>
    /// https://www.codedad.net/2017/08/26/asp-net-core-2-response-logging-2/
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            using (MemoryStream loggableResponseStream = new MemoryStream())
            {
                Stream originalResponseStream = context.Response.Body;
                context.Response.Body = loggableResponseStream;

                try
                {
                    context.Request.EnableRewind();
                    // Log request
                    _logger.LogInformation(await FormatRequest(context.Request));
                    context.Request.Body.Position = 0;
                    await _next(context);

                    // Log response
                    _logger.LogInformation(await FormatResponse(loggableResponseStream, context.Response.StatusCode));
                    //reset the stream position to 0
                    loggableResponseStream.Seek(0, SeekOrigin.Begin);
                    await loggableResponseStream.CopyToAsync(originalResponseStream);
                }
                catch (Exception ex)
                {
                    // Log error
                    _logger.LogError(ex, ex.Message);

                    //allows exception handling middleware to deal with things
                    throw;
                }
                finally
                {
                    //Reassign the original stream. If we are re-throwing an exception this is important as the exception handling middleware will need to write to the response stream.
                    context.Response.Body = originalResponseStream;
                }
            }
        }

        private static async Task<string> FormatRequest(HttpRequest request)
        {
            Stream body = request.Body;
            byte[] buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            string bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body = body;

            var messageObjToLog = new
            {
                scheme = request.Scheme,
                host = request.Host,
                path = request.Path,
                queryString = request.Query,
                requestBody = bodyAsText
            };

            return JsonConvert.SerializeObject(messageObjToLog);
        }

        private static async Task<string> FormatResponse(Stream loggableResponseStream, int statusCode)
        {
            byte[] buffer = new byte[loggableResponseStream.Length];
            await loggableResponseStream.ReadAsync(buffer, 0, buffer.Length);

            var messageObjectToLog = new
            {
                responseBody = Encoding.UTF8.GetString(buffer),
                statusCode = statusCode
            };

            return JsonConvert.SerializeObject(messageObjectToLog);
        }
    }
}
