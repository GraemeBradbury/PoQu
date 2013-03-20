using System;

namespace SiliconShark.PoQu
{
    using Interfaces;
    using Microsoft.WindowsAzure.Storage.Queue;

    internal class CloudQueueWrapper : IQueue
    {
        private readonly CloudQueue _queue;

        public CloudQueueWrapper(CloudQueue queue)
        {
            _queue = queue;
        }

        public void Put(CloudQueueMessage message)
        {
            _queue.AddMessage(message);
        }

        public CloudQueueMessage Get()
        {
            return _queue.GetMessage();
        }
    }
}