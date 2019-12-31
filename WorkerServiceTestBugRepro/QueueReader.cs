using System.Threading.Tasks;

namespace WorkerServiceTestBugRepro
{
    public class QueueReader : IQueueReader
    {
        public async Task<bool> HasMessage()
        {
            await Task.Delay(100); // simulate real delay

            return true;
        }
    }
}
