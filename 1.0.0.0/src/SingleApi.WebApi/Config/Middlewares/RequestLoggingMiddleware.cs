using System.Buffers;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using SfpCoreLib.Domain.Extensions;
using SingleApi.Infrastructure.Wrappers;

namespace SingleApi.WebApi.Config.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly NetworkHelper _networkHelper;

        public RequestLoggingMiddleware(
            RequestDelegate next,
            NetworkHelper networkHelper)
        {
            _next = next.NotNull(nameof(next));
            _networkHelper = networkHelper.NotNull(nameof(networkHelper));
        }

        public async Task Invoke(HttpContext context, WSLoggerWrapper WSLoggerWrapper)
        {
            WSLoggerWrapper.NotNull(nameof(WSLoggerWrapper));
            context.Request.EnableBuffering();

            byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);
            var requestBody = new StringBuilder();

            while (true)
            {
                var bytesRemaining = await context.Request.Body.ReadAsync(buffer, offset: 0, buffer.Length);
                if (bytesRemaining == 0)
                {
                    break;
                }

                // Append the encoded string into the string builder.
                var encodedString = Encoding.UTF8.GetString(buffer, 0, bytesRemaining);
                requestBody.Append(encodedString);
            }

            ArrayPool<byte>.Shared.Return(buffer);
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            
            var response = string.Empty;
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            Stream originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                response = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                await responseBody.CopyToAsync(originalBodyStream);
            }

            WSLoggerWrapper.LogRequest(
                context.Request.GetDisplayUrl(),
                context.Request.Method,
                context.Request.ContentType ?? string.Empty,
                "Unicode(UTF - 8)",
                requestBody.ToString(),
                response,
                false);
        }
    }
}