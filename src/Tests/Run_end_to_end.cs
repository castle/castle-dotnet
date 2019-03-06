using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Net.Config;
using Castle.Net.Messages;
using Xunit;

namespace Tests
{
    public class Run_end_to_end
    {
        [Fact(Skip = "manual")]
        public async Task Send()
        {
            var castleOptions = new CastleOptions()
            {
                ApiSecret = "gQpLzXqezkdHpbpd8vUzTzNdKQL3RotD",
                Timeout = 1000
            };

            var actionRequest = new ActionRequest()
            {
                Event = "testing",
                UserId = "123",
                DeviceToken = "ey...",
                Context = new RequestContext()
                {
                    Ip = "111.111.111.111",
                    ClientId = "123",
                    Headers = new Dictionary<string, string>
                    {
                        ["User-Agent"] = "x"
                    }
                },
            };


            var castle = new Castle.Net.Castle(castleOptions);
            await castle.Authenticate(actionRequest);

        }
    }
}
