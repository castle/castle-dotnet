namespace Castle.Messages.Requests
{
    public class ImpersonateStartRequest
    {
        public string UserId { get; set; }

        public string Impersonator { get; set; }

        public RequestContext Context { get; set; } = new RequestContext();
    }
}