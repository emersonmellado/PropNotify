using System;
using System.Linq.Expressions;
using System.Reflection;

namespace lib.ObservablePattern
{
    public interface IPropNotify<T>
    {
        void OnNotify(T obj, PropertyInfo propertyInfo);
        Expression<Func<T, object>>[] PropsMonitored { get; set; }
    }
}
