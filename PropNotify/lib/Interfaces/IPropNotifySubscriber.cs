using System;

namespace lib.Interfaces
{
    public interface IPropNotifySubscriber<T>
    {
        IDisposable Subscribe(IPropNotify<T> observer);
    }
}