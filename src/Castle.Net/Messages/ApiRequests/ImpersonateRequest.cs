using Castle.Messages.SdkRequests;

namespace Castle.Messages.ApiRequests
{
    internal class ImpersonateRequest
    {
        public ImpersonateRequest()
        {
            
        }

        public ImpersonateRequest(ImpersonateStartRequest startRequest)
        {
            UserId = startRequest.UserId;
            Impersonator = startRequest.Impersonator;
            Context = startRequest.Context;
        }

        public ImpersonateRequest(ImpersonateEndRequest endRequest)
        {
            UserId = endRequest.UserId;
        }

        public string UserId { get; set; }

        public string Impersonator { get; set; }

        public RequestContext Context { get; set; }
    }
}
