using System.Net.Http;
using System.Threading.Tasks;
using Castle.Net.Infrastructure.Exceptions;

namespace Castle.Net.Infrastructure.Extensions
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
