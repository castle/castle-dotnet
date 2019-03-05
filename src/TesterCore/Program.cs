using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Castle.Net.Config;
using Castle.Net.Infrastructure;
using Castle.Net.Messages;

namespace TesterCore
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var castle = new Castle.Net.Castle(new CastleOptions()
            //{
            //    ApiSecret = "gQpLzXqezkdHpbpd8vUzTzNdKQL3RotD",
            //});

            //castle.Authenticate(new ActionRequest()
            //{
            //    Event = "testing",
            //    UserId = "123",
            //    DeviceToken = "ey..."
            //}).Wait();

            var client = new HttpClient();
            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(":gQpLzXqezkdHpbpd8vUzTzNdKQL3RotD"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
            //var response = await client.GetAsync("https://api.castle.io/v1/devices/1");

            //var content = new StringContent(
            //    @"{
            //    ""event"" : ""testing"",
            //    ""user_id"": ""123"",
            //    ""device_token"": ""ey...""
            //}",
            //    Encoding.UTF8,
            //    "application/json");

            var content = new StringContent(JsonConvertForCastle.SerializeObject(new ActionRequest()
            {
                Event = "testing",
                UserId = "123",
                DeviceToken = "ey..."
            }), Encoding.UTF8, "application/json");

            client.BaseAddress = new Uri("https://api.castle.io");
            var response = await client.PostAsync("/v1/authenticate", content);
        }
    }
}
