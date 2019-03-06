using Newtonsoft.Json;

namespace Castle.Net.Messages
{
    public class AuthenticateResponse
    {
        public ActionType Action { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("device_token")]
        public string DeviceToken { get; set; }

        public bool Failover { get; set; }

        [JsonProperty("failover_reason")]
        public string FailoverReason { get; set; }
    }
}
