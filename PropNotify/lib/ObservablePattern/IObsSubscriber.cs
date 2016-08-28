using System;

namespace lib.ObservablePattern
{
    public interface IObsSubscriber<T>
    {
        IDisposable Subscribe(IObsNotify<T> observer);
    }
}