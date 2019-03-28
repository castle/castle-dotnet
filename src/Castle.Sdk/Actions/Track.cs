using System;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Messages.Responses;

namespace Castle.Actions
{
    internal static class Track
    {
        public static async Task<VoidResponse> Execute(
            Func<Task<VoidResponse>> send,
            CastleConfiguration configuration)
        {
            return await send();
        }
    }
}
