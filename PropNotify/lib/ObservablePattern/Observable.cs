using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace lib.ObservablePattern
{
    public class Observable<T> : IObsSubscriber<T>
    {
        private readonly List<IObsNotify<T>> _observers;
        private readonly List<T> _list;

        public Observable()
        {
            _observers = new List<IObsNotify<T>>();
            _list = new List<T>();
        }

        protected void Notify(T obj)
        {

            Parallel.Invoke(() => _observers.ForEach(o =>
            {
                var props = o.PropsMonitored;
                var current = _list.FirstOrDefault(f => f.Equals(obj));
                if (current != null)
                {
                    foreach (var expression in props)
                    {
                        var p = GetProperty(expression);
                        var pOldValue = p.GetValue(current, null);
                        var pNewValue = p.GetValue(obj, null);
                        if (!pNewValue.Equals(pOldValue))
                            o.OnNotify(obj, p);
                    }
                }
            }));
            _list.AddOrUpdate(obj);
        }

        public IDisposable Subscribe(IObsNotify<T> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
            return new Unsubscriber(_observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObsNotify<T>> _observers;
            private readonly IObsNotify<T> _observer;

            public Unsubscriber(List<IObsNotify<T>> observers, IObsNotify<T> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                {
                    _observers.Remove(_observer);
                }
            }
        }

        public static PropertyInfo GetProperty<T>(Expression<Func<T, object>> expression)
        {
            MemberExpression memberExpression = null;

            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new ArgumentException("Not a member access", nameof(expression));
            }

            return memberExpression.Member as PropertyInfo;
        }
    }
}
