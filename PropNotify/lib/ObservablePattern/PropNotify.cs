using System;
using System.Linq.Expressions;
using System.Reflection;

namespace lib.ObservablePattern
{
    public abstract class PropNotify<T> : IPropNotify<T>
    {
        public Expression<Func<T, object>>[] PropsMonitored { get; set; }

        protected PropNotify(params Expression<Func<T, object>>[] props)
        {
            if (props == null)
                throw new ArgumentNullException(nameof(props));

            PropsMonitored = props;
        }

        public abstract void OnNotify(T mod, PropertyInfo propertyInfo);
    }
}
