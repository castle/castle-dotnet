using Newtonsoft.Json.Linq;

namespace Castle.Messages.Responses
{
    public class Verdict : IHasJson
    {
        public ActionType Action { get; set; }
        public Policy Policy { get; set; }
        public string UserId { get; set; }

        public string DeviceToken { get; set; }

        public bool Failover { get; set; }

        public string FailoverReason { get; set; }

        public JObject Internal { get; set; }
    }
}
