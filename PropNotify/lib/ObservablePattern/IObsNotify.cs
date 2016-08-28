using System;
using System.Linq.Expressions;
using System.Reflection;

namespace lib.ObservablePattern
{
    public interface IObsNotify<T>
    {
        void OnNotify(T mod, PropertyInfo propertyInfo);
        Expression<Func<T, object>>[] PropsMonitored { get; set; }
    }
}
