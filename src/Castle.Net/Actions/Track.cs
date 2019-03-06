using System;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Messages;

namespace Castle.Actions
{
    internal static class Track
    {
        public static async Task<VoidResponse> Execute(
            Func<ActionRequest, Task<VoidResponse>> send,
            ActionRequest request,
            CastleOptions options)
        {
            var alteredRequest = request.ShallowCopy();
            if (request.Context != null)
            {
                var scrubbed = HeaderScrubber.Scrub(request.Context.Headers, options.Whitelist, options.Blacklist);
                alteredRequest.Context = request.Context.WithHeaders(scrubbed);
            }

            return await send(alteredRequest);
        }
    }
}
