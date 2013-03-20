using System;

namespace SiliconShark.PoQu
{
    using Extensions;
    using Interfaces;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.WindowsAzure.Storage;

    public class QueueManager
    {
        private IMessageBuilder _messageBuilder;
        private IQueueBuilder _queueBuilder;

        public QueueManager(IServiceLocator serviceLocator)
        {
            _queueBuilder = serviceLocator.SafeGetInstance<IQueueBuilder>();
            if (_queueBuilder == null)
            {
                //if there is no storage account we let the service locator throw 
                var storageAccount = serviceLocator.GetInstance<CloudStorageAccount>();
                var queueNamer =
                    serviceLocator.GetInstanceWithDefault<IQueueNamer>(() => new AzureQueueNamer());

                _queueBuilder = new QueueBuilder(queueNamer, storageAccount);
            }

            _messageBuilder = serviceLocator.SafeGetInstance<IMessageBuilder>();
        }

        protected QueueManager()
        {
        }

        public static QueueManager Create(string storageConnectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var queueNamer = new AzureQueueNamer();
            var queueBuilder = new QueueBuilder(queueNamer, storageAccount);
            var messageBuilder = new JsonBasedMessageBuilder();

            var manager = new QueueManager {_messageBuilder = messageBuilder, _queueBuilder = queueBuilder};

            return manager;
        }

        public void Put<T>(T message, bool onPoisonQueue = false)
        {
            var queue = _queueBuilder.GetQueueFor(message, onPoisonQueue);
            var queueMessage = _messageBuilder.CreateMessage(message);
            queue.Put(queueMessage);
        }

        public T Get<T>(bool fromPoisonQueue = false)
        {
            var messageType = typeof (T);
            var queue = _queueBuilder.GetQueueFor(messageType, fromPoisonQueue);
            var message = _messageBuilder.RetrieveMessagePayload<T>(queue.Get());
            return message;
        }
    }
}