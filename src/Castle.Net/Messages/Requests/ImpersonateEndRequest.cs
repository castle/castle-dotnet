namespace Castle.Messages.Requests
{
    public class ImpersonateEndRequest
    {
        public string UserId { get; set; }

        public RequestContext Context { get; set; } = new RequestContext();
    }
}