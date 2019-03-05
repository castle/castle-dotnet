using System.Threading.Tasks;
using Castle.Net.Infrastructure;
using Castle.Net.Messages;

namespace Castle.Net.Actions
{
    internal class Authenticate
    {
        private readonly IMessageSender _messageSender;

        public Authenticate(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        public async Task<ActionResponse> Execute(ActionRequest request)
        {
            await _messageSender.Post(request, "/v1/authenticate");
            return new ActionResponse();
        }
    }
}
