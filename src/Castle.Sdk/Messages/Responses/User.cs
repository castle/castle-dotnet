using System;
using System.Collections.Generic;

namespace Castle.Messages.Responses
{
    public class User
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime LastSeenAt { get; set; }

        public DateTime? FlaggedAt { get; set; }

        public float Risk { get; set; }

        public int LeaksCount { get; set; }

        public int DevicesCount { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Phone { get; set; }

        public Address Address { get; set; }

        public IDictionary<string, string> CustomAttributes { get; set; }
    }
}
