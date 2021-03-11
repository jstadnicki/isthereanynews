using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Itan.Api.Middleware
{
    public class ItanExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (ItanValidationException e)
            {
                var status = HttpStatusCode.BadRequest;
                var resultToSerialize = new {e.Message, e.StackTrace};
                var result = JsonConvert.SerializeObject(resultToSerialize);
                context.Response.StatusCode = (int) status;
                await context.Response.WriteAsync(result);
            }
        }
    }

    public class ItanValidationException : Exception
    {
    }
}