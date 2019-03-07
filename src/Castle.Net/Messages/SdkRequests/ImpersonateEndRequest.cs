namespace Castle.Messages.SdkRequests
{
    public class ImpersonateEndRequest
    {
        public string UserId { get; set; }

        public RequestContext Context { get; set; }
    }
}