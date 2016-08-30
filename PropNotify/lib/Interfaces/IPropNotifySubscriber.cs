using System;

namespace lib.Interfaces
{
    public interface IPropNotifySubscriber<T> where T : IEquatable<T>
    {
        IDisposable Subscribe(IPropNotify<T> observer);
    }
}