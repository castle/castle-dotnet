using System;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Infrastructure.Exceptions;
using Castle.Messages;
using Castle.Messages.Responses;

namespace Castle.Actions
{
    internal static class Log
    {
        public static async Task<VoidResponse> Execute(
            Func<Task<VoidResponse>> send,
            CastleConfiguration configuration)
        {
            return await send();
        }
    }
}
