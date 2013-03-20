using System;

namespace SiliconShark.PoQu.Interfaces
{
    using Microsoft.WindowsAzure.Storage.Queue;

    public interface IMessageBuilder
    {
        CloudQueueMessage CreateMessage<T>(T message);
        T RetrieveMessagePayload<T>(CloudQueueMessage cloudQueueMessage);
    }
}