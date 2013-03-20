using System;

namespace SiliconShark.PoQu.Interfaces
{
    public interface IQueueBuilder
    {
        IQueue GetQueueFor(dynamic message, bool onPoisonQueue = false);
        IQueue GetQueueFor(Type messageType, bool onPoisonQueue = false);
    }
}