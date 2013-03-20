using System;

namespace SiliconShark.PoQu
{
    using System.IO;
    using System.Runtime.Serialization.Json;
    using Interfaces;
    using Microsoft.WindowsAzure.Storage.Queue;

    internal class JsonBasedMessageBuilder : IMessageBuilder
    {
        public T RetrieveMessagePayload<T>(CloudQueueMessage cloudQueueMessage)
        {
            var serializer = new DataContractJsonSerializer(typeof (T));
            using (var memoryStream = new MemoryStream(cloudQueueMessage.AsBytes))
            {
                var readObject = serializer.ReadObject(memoryStream);
                return (T) readObject;
            }
        }

        public CloudQueueMessage CreateMessage<T>(T message)
        {
            var serializer = new DataContractJsonSerializer(typeof (T));
            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, message);
                string messageString;
                using (var streamReader = new StreamReader(memoryStream))
                {
                    messageString = streamReader.ReadToEnd();
                }

                return new CloudQueueMessage(messageString);
            }
        }
    }
}