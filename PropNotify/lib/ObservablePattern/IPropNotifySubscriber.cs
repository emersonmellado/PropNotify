using System;

namespace lib.ObservablePattern
{
    public interface IPropNotifySubscriber<T>
    {
        IDisposable Subscribe(IPropNotify<T> observer);
    }
}