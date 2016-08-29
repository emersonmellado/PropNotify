using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using lib.Interfaces;

namespace lib.Common
{
    public class Observable<T> : IPropNotifySubscriber<T>
    {
        private readonly List<IPropNotify<T>> _observers;
        private readonly List<T> _list;

        public Observable()
        {
            _observers = new List<IPropNotify<T>>();
            _list = new List<T>();
        }

        protected void Notify(T obj)
        {

            Parallel.Invoke(() => _observers.ForEach(o =>
            {
                var props = o.PropsToObserver;
                if (props != null && props.Any())
                {
                    var current = _list.FirstOrDefault(f => f.Equals(obj));
                    if (current == null) return;
                    foreach (var expression in props)
                    {
                        var p = GetProperty(expression);
                        var pOldValue = p.GetValue(current, null);
                        var pNewValue = p.GetValue(obj, null);
                        if (!pNewValue.Equals(pOldValue))
                            o.OnNotify(obj, p.Name);
                    }
                }
                var conditions = o.ConditionsToObserver;
                if (conditions != null && conditions.Any())
                {
                    var current = _list.FirstOrDefault(f => f.Equals(obj));
                    if (current == null) return;
                    foreach (var expression in conditions)
                    {
                        var func = expression.Compile();
                        if (func(obj))
                        {
                            var expBody = expression.Body.ToString();
                            o.OnNotify(obj, expBody);
                        }
                    }
                }
            }));
            _list.AddOrUpdate(obj);
        }

        public IDisposable Subscribe(IPropNotify<T> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
            return new Unsubscriber(_observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IPropNotify<T>> _observers;
            private readonly IPropNotify<T> _observer;

            public Unsubscriber(List<IPropNotify<T>> observers, IPropNotify<T> observer)
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
