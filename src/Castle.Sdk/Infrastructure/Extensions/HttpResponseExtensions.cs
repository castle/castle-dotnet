using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Castle.Infrastructure.Exceptions;
using Castle.Infrastructure.Json;

namespace Castle.Infrastructure.Extensions
{
    internal static class HttpResponseExtensions
    {
        public static async Task<Exception> ToCastleException(this HttpResponseMessage message, string requestUri)
        {

            if (message.StatusCode == HttpStatusCode.NotFound)
            {
                throw new CastleNotFoundException("Not Found", requestUri, message.StatusCode);
            }
            var content = await message.Content.ReadAsStringAsync();
            try
            {
                var parsedContent = JsonForCastle.DeserializeObject<Dictionary<string, string>>(content);
                if (parsedContent["type"] != null)
                {
                    if (parsedContent["type"] == "invalid_request_token")
                    {
                        throw new CastleInvalidTokenException(parsedContent["message"], requestUri, message.StatusCode);
                    }
                    throw new CastleInvalidParametersException(parsedContent["message"], requestUri, message.StatusCode);
                }
            }
            catch (JsonException)
            {
                return new CastleInternalException(content, requestUri, message.StatusCode);
            }
            catch (Exception)
            {
                throw new CastleInvalidParametersException(content, requestUri, message.StatusCode);
            }



            return new CastleInternalException(content, requestUri, message.StatusCode);
        }
    }
}
