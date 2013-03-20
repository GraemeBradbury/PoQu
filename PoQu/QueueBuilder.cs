using System;

namespace SiliconShark.PoQu
{
    using Interfaces;
    using Microsoft.WindowsAzure.Storage;

    public class QueueBuilder : IQueueBuilder
    {
        private readonly IQueueNamer _queueNamer;
        private readonly CloudStorageAccount _storageAccount;

        public QueueBuilder(IQueueNamer queueNamer, CloudStorageAccount storageAccount)
        {
            _queueNamer = queueNamer;
            _storageAccount = storageAccount;
        }

        public IQueue GetQueueFor(dynamic message, bool onPoisonQueue = false)
        {
            var queueName = _queueNamer.CreateQueueNameFor(message, onPoisonQueue);
            return GetQueue(queueName);
        }

        public IQueue GetQueueFor(Type messageType, bool onPoisonQueue = false)
        {
            var queueName = _queueNamer.CreateQueueNameFor(messageType, onPoisonQueue);
            return GetQueue(queueName);
        }

        private IQueue GetQueue(string queueName)
        {
            var queueReference = _storageAccount.CreateCloudQueueClient().GetQueueReference(queueName);
            queueReference.CreateIfNotExists();

            return new CloudQueueWrapper(queueReference);
        }
    }
}