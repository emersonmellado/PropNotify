using System;
using System.Linq.Expressions;
using System.Reflection;

namespace lib.Interfaces
{
    public interface IPropNotify<T>
    {
        void OnNotify(T obj, string triggeredBy);
        Expression<Func<T, object>>[] PropsToObserver { get; set; }
        Expression<Func<T, bool>>[] ConditionsToObserver { get; set; }
    }
}
