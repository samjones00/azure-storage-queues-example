using System.Threading.Tasks;

namespace QueueApp.Interfaces
{
    public interface IQueueService
    {
        Task DeleteEmptyQueue();
        Task Insert(string message);
        Task<string> Retrieve();
    }
}
