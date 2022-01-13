namespace Castle
{
    /// <summary>
    /// Castle tracking event constants
    /// </summary>
    public static class Events
    {
        public const string LoginSucceeded = "$login.succeeded";
        public const string LoginFailed = "$login.failed";
        public const string LogoutSucceeded = "$logout.succeeded";
        public const string ProfileUpdateSucceeded = "$profile_update.succeeded";
        public const string ProfileUpdateFailed = "$profile_update.failed";
        public const string RegistrationSucceeded = "$registration.succeeded";
        public const string RegistrationFailed = "$registration.failed";
        public const string PasswordResetSucceeded = "$password_reset.succeeded";
        public const string PasswordResetFailed = "$password_reset.failed";
        public const string PasswordResetRequestSucceeded = "$password_reset_request.succeeded";
        public const string PasswordResetRequestFailed = "$password_reset_request.failed";
        public const string IncidentMitigated = "$incident.mitigated";
        public const string ReviewEscalated = "$review.escalated";
        public const string ReviewResolved = "$review.resolved";
        public const string ChallengeRequested = "$challenge.requested";
        public const string ChallengeSucceeded = "$challenge.succeeded";
        public const string ChallengeFailed = "$challenge.failed";
        public const string Challenge = "$challenge";
        public const string ProfileUpdate = "$profile_update";
        public const string PasswordResetRequest = "$password_reset_request";
    }
}
