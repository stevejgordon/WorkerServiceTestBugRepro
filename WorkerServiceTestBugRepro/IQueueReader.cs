using System.Threading.Tasks;

namespace WorkerServiceTestBugRepro
{
    public interface IQueueReader
    {
        Task<bool> HasMessage();
    }
}