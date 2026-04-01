using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Castle.Messages.Responses
{

    public class ScoreItem
    {
        public float Score { get; set; }
    }

    public class RiskResponse
    {
        public DeviceItem Device { get; set; }
        public float Risk { get; set; }

        public Policy Policy { get; set; }
        public JObject Signals { get; set; }

        public Dictionary<string, ScoreItem> Scores { get; set; }

        // FailOver
        public ActionType Action { get; set; }

        public bool Failover { get; set; }

        public string FailoverReason { get; set; }

    }

}