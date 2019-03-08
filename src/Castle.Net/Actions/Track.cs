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
            CastleOptions options)
        {
            var apiRequest = request.PrepareApiCopy(options.Whitelist, options.Blacklist);

            return await send(apiRequest);
        }
    }
}
