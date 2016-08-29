using System;
using System.Linq.Expressions;
using lib.Interfaces;

namespace lib.Common
{
    public abstract class PropNotify<T> : IPropNotify<T>
    {
        public Expression<Func<T, object>>[] PropsToObserver { get; set; }
        public Expression<Func<T, bool>>[] ConditionsToObserver { get; set; }

        public void AddConditionToObserver(params Expression<Func<T, bool>>[] conditionsToObserver)
        {
            if (conditionsToObserver == null)
                throw new ArgumentNullException(nameof(conditionsToObserver));
            ConditionsToObserver = conditionsToObserver;
        }

        public void AddPropertiesToObserver(params Expression<Func<T, object>>[] propsToObserver)
        {
            if (propsToObserver == null)
                throw new ArgumentNullException(nameof(propsToObserver));
            PropsToObserver = propsToObserver;
        }

        public abstract void OnNotify(T obj, string triggeredBy);
    }
}
