﻿using System;

namespace Castle.Messages
{
    public class Device
    {
        public string Token { get; set; }

        public float Risk { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastSeenAt { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public DateTime? EscalatedAt { get; set; }

        public DateTime? MitigatedAt { get; set; }

        public DeviceContext Context { get; set; }
    }
}