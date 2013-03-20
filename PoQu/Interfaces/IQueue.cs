using System;

namespace SiliconShark.PoQu.Interfaces
{
    using Microsoft.WindowsAzure.Storage.Queue;

    public interface IQueue
    {
        void Put(CloudQueueMessage message);
        CloudQueueMessage Get();
    }
}