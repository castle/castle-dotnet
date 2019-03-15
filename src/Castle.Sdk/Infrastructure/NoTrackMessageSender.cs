using System.Threading.Tasks;

namespace Castle.Infrastructure
{
    internal class NoTrackMessageSender : IMessageSender
    {
        public async Task<TResponse> Post<TResponse>(string endpoint, object payload) where TResponse : class, new()
        {
            return await Task.FromResult(new TResponse());
        }

        public async Task<TResponse> Get<TResponse>(string endpoint) where TResponse : class, new()
        {
            return await Task.FromResult(new TResponse());
        }

        public async Task<TResponse> Put<TResponse>(string endpoint) where TResponse : class, new()
        {
            return await Task.FromResult(new TResponse());
        }

        public async Task<TResponse> Delete<TResponse>(string endpoint, object payload) where TResponse : class, new()
        {
            return await Task.FromResult(new TResponse());
        }
    }
}
