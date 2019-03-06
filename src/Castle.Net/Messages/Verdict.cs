namespace Castle.Messages
{
    public class Verdict
    {
        public ActionType Action { get; set; }

        public string UserId { get; set; }

        public string DeviceToken { get; set; }

        public bool Failover { get; set; }

        public string FailoverReason { get; set; }
    }
}
