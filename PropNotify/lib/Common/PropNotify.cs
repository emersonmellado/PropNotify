using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using lib.Interfaces;

namespace lib.Common
{
    public abstract class PropNotify<T> : IPropNotify<T> where T : IEquatable<T>
    {
        protected PropNotify()
        {
            Actions = new List<ActionHolder<T>>();
        }
        public List<ActionHolder<T>> Actions { get; set; }

        public void AddWatchCondition(params Expression<Func<T, bool>>[] conditionsToObserver)
        {
            if (conditionsToObserver == null)
                throw new ArgumentNullException(nameof(conditionsToObserver));
            foreach (var expression in conditionsToObserver)
            {
                Actions.AddCondition(OnNotify, expression);
            }
        }

        public void AddWatchProperty(params Expression<Func<T, object>>[] propsToObserver)
        {
            if (propsToObserver == null)
                throw new ArgumentNullException(nameof(propsToObserver));
            foreach (var expression in propsToObserver)
            {
                Actions.AddProperty(OnNotify, expression);
            }
        }

        public abstract void OnNotify(T obj, string triggeredBy);

    }
}
