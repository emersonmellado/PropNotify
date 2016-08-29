using System;
using System.Linq.Expressions;
using System.Reflection;

namespace lib.Interfaces
{
    public interface IPropNotify<T>
    {
        void OnNotify(T obj, PropertyInfo propertyInfo);
        Expression<Func<T, object>>[] PropsMonitored { get; set; }
    }
}
