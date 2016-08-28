using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ThermometerLibrary.ObservablePattern
{
    public interface IObsNotify<T>
    {
        void OnNotify(T mod, PropertyInfo propertyInfo);
        Expression<Func<T, object>>[] PropsMonitored { get; set; }
    }

    public interface IObsSubscriber<T>
    {
        IDisposable Subscribe(IObsNotify<T> observer);
    }
}
