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

            if (message.StatusCode == HttpStatusCode.NotFound || message.StatusCode == HttpStatusCode.BadRequest || message.StatusCode == HttpStatusCode.Unauthorized || message.StatusCode == HttpStatusCode.Forbidden || (int)message.StatusCode == 419)
            {
                throw new CastleClientErrorException("Invalid response from Castle API", requestUri, message.StatusCode);
            }
            var content = await message.Content.ReadAsStringAsync();
            if ((int)message.StatusCode == 422)
            {
                try
                {
                    var parsedContent = JsonForCastle.DeserializeObject<Dictionary<string, string>>(content);
                    if (parsedContent.ContainsKey("type"))
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
                throw new CastleInvalidParametersException(content, requestUri, message.StatusCode);
            }

            return new CastleInternalException(content, requestUri, message.StatusCode);
        }
    }
}
