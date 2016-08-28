using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ThermometerLibrary.ObservablePattern
{
    public abstract class ObsNotify<T> : IObsNotify<T>
    {
        public Expression<Func<T, object>>[] PropsMonitored { get; set; }

        protected ObsNotify(params Expression<Func<T, object>>[] props)
        {
            if (props == null)
                throw new ArgumentNullException(nameof(props));

            PropsMonitored = props;
        }

        public abstract void OnNotify(T mod, PropertyInfo propertyInfo);
    }
}
