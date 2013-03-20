using System;

namespace SiliconShark.PoQu.Interfaces
{
    internal interface IMessageSerializer
    {
        byte[] Serialize(object message);
    }
}