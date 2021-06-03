using System;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Infrastructure;
using Newtonsoft.Json.Linq;

namespace Castle.Actions
{
    internal static class Log
    {
        public static async Task<JObject> Execute(
            Func<Task<JObject>> send,
            CastleConfiguration configuration,
            IInternalLogger logger)
        {
            return await send();
        }
    }
}
