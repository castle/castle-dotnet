using System.Threading.Tasks;
using Castle.Net.Messages;

namespace Castle.Net.Infrastructure
{
    internal interface IMessageSender
    {
        Task<TResponse> Post<TResponse>(ActionRequest payload, string endpoint);
    }
}