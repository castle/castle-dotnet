using System.Net.Http;
using System.Text;
using Castle.Infrastructure.Json;

namespace Castle.Infrastructure.Extensions
{
    internal static class ObjectExtensions
    {
        public static StringContent ToHttpContent(this object obj)
        {
            return new StringContent(
                JsonForCastle.SerializeObject(obj),
                Encoding.UTF8,
                "application/json");
        }
    }
}
