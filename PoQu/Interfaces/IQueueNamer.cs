using System;

namespace SiliconShark.PoQu.Interfaces
{
    public interface IQueueNamer
    {
        string CreateQueueNameFor(dynamic message, bool isPoisonQueue = false);
        string CreateQueueNameFor(Type messageType, bool isPoisonQueue = false);
    }
}