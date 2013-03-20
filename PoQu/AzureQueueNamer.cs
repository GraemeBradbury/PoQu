using System;

namespace SiliconShark.PoQu
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Interfaces;

    public class AzureQueueNamer : IQueueNamer
    {
        private static readonly Regex Splitter = new Regex(@"[A-Z][a-z]+", RegexOptions.Compiled);

        private static readonly ConcurrentDictionary<Type, string> NameStore = new ConcurrentDictionary<Type, string>();

        public string CreateQueueNameFor(dynamic message, bool isPoisonQueue = false)
        {
            Type key = message as Type ?? message.GetType();

            return CreateQueueNameFor(key);
        }

        public string CreateQueueNameFor(Type messageType, bool isPoisonQueue = false)
        {
            if (NameStore.ContainsKey(messageType))
            {
                return NameStore[messageType];
            }

            var name = TransformIntoQueueName(messageType.Name);
            NameStore[messageType] = name;
            return name;
        }

        private string TransformIntoQueueName(string typeName)
        {
            var builder = new StringBuilder();
            builder.Append("queue-");

            Splitter.Matches(typeName)
                    .OfType<Match>()
                    .Select(m => m.Value.ToLowerInvariant())
                    .Except(new[] {"message"})
                    .ToList()
                    .ForEach(s => builder.AppendFormat("{0}-", s));

            var name = builder.ToString();

            var index = name.Length > 63 ? 62 : name.Length - 1;

            while (name[index] == '-')
            {
                index--;
            }
            return name.Substring(0, index + 1);
        }
    }
}