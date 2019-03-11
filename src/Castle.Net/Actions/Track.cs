using System;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Messages.Requests;
using Castle.Messages.Responses;

namespace Castle.Actions
{
    internal static class Track
    {
        public static async Task<VoidResponse> Execute(
            Func<ActionRequest, Task<VoidResponse>> send,
            ActionRequest request,
            CastleConfiguration configuration)
        {
            var apiRequest = request.PrepareApiCopy(configuration.Whitelist, configuration.Blacklist);

            return await send(apiRequest);
        }
    }
}
