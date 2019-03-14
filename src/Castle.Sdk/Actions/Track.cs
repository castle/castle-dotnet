using System;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Messages.Responses;
using Newtonsoft.Json.Linq;

namespace Castle.Actions
{
    internal static class Track
    {
        public static async Task<VoidResponse> Execute(
            Func<JObject, Task<VoidResponse>> send,
            JObject request,
            CastleConfiguration configuration)
        {
            return await send(request);
        }
    }
}
