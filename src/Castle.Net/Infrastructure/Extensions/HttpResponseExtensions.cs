using System.Net.Http;
using System.Threading.Tasks;
using Castle.Infrastructure.Exceptions;

namespace Castle.Infrastructure.Extensions
{
    internal static class HttpResponseExtensions
    {
        public static async Task<CastleInternalException> ToCastleException(this HttpResponseMessage message, string requestUri)
        {
            var content = await message.Content.ReadAsStringAsync();
            return new CastleInternalException(requestUri, content, message.StatusCode);
        }
    }
}
