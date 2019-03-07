namespace Castle.Messages.SdkRequests
{
    public class ImpersonateStartRequest
    {
        public string UserId { get; set; }

        public string Impersonator { get; set; }

        public RequestContext Context { get; set; }
    }
}