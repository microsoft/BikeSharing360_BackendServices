using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyBikes.Services.Profiles.Formatters
{
    public class TextPlainInputFormatter : IInputFormatter
    {
        private const string TextContentType = "text/plain";

        public bool CanRead(InputFormatterContext context)
        {
            var typeValid = context.ModelType == typeof(string)
                || context.ModelType == typeof(Guid);

            var contentTypeValid =
                context.HttpContext.Request.ContentType == TextContentType;

            return typeValid && contentTypeValid;
        } 

        public async Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            var destIsGuid = context.ModelType == typeof(Guid);
            if (request.ContentLength == 0)
            {
                if (destIsGuid)
                {
                    return InputFormatterResult.Success(Guid.Empty);
                }
                else
                {
                    return InputFormatterResult.Success(null);
                }
            }

            using (var reader = new StreamReader(request.Body))
            {

                var str = await reader.ReadToEndAsync();
                if (destIsGuid)
                {
                    return ParseGuidResult(str);
                }
                else
                {
                    return InputFormatterResult.Success(str);
                }
            }


        }

        private InputFormatterResult ParseGuidResult(string str)
        {
            Guid guid;
            var ok = Guid.TryParse(str, out guid);
            if (ok)
            {
                return InputFormatterResult.Success(guid);
            }
            else
            {
                return InputFormatterResult.Failure();
            }
        }
    }
}
