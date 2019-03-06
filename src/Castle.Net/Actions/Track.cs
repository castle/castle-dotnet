using System.Threading.Tasks;
using Castle.Net.Config;
using Castle.Net.Infrastructure;
using Castle.Net.Messages;

namespace Castle.Net.Actions
{
    internal static class Track
    {
        public static async Task<VoidResponse> Execute(
            IMessageSender sender,
            ActionRequest request,
            CastleOptions options)
        {
            var alteredRequest = request.ShallowCopy();
            if (request.Context != null)
            {
                var scrubbed = HeaderScrubber.Scrub(request.Context.Headers, options.Whitelist, options.Blacklist);
                alteredRequest.Context = request.Context.WithHeaders(scrubbed);
            }

            return await sender.Post<VoidResponse>(alteredRequest, "/v1/track");
        }
    }
}
