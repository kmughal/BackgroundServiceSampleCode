using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace background_queue
{
    public class BackgroundQueue   { 

        private ConcurrentQueue<Func<CancellationToken, Task>> _workingItem = new ConcurrentQueue<Func<CancellationToken, Task>>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public async Task<Func<CancellationToken,Task>> DequeueAsync(CancellationToken cancellationToken) {
            await _signal.WaitAsync(cancellationToken);

            _workingItem.TryDequeue(out var item);
            return item;
        }

        public void QueueBackgroundWork(Func<CancellationToken, Task> workItem) { 
            if (workItem == null) throw new ArgumentNullException(nameof(workItem));
            _workingItem.Enqueue(workItem);
            _signal.Release();
        }
        
       
    }
}
