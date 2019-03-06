using System.Threading.Tasks;
using Castle.Messages;

namespace Castle.Infrastructure
{
    internal interface IMessageSender
    {
        Task<TResponse> Post<TResponse>(ActionRequest payload, string endpoint) where TResponse : class, new(); 
        
        Task<TResponse> Get<TResponse>(string endpoint) where TResponse : class, new();

        Task<TResponse> Put<TResponse>(string endpoint) where TResponse : class, new();
    }
}