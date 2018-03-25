using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using lib.Interfaces;

namespace lib.Common
{
    public class Observable<T> : IPropNotifySubscriber<T> where T : IEquatable<T>
    {
        private readonly List<IPropNotify<T>> _observers;
        private readonly List<T> _list;

        public Observable()
        {
            _observers = new List<IPropNotify<T>>();
            _list = new List<T>();
        }

        public List<T> GetList() => new List<T>(_list);

        public List<IPropNotify<T>> GetObservers() => new List<IPropNotify<T>>(_observers);

        protected void Notify(T obj)
        {
            Parallel.Invoke(() => _observers.ForEach(o =>
            {
                var currentObs = _list.FirstOrDefault(f => !EqualityComparer<T>.Default.Equals(f, obj));
                var propertyActions = o.Actions.Where(w => !w.ConditionExp).ToList();
                if (propertyActions.Any())
                {
                    if (currentObs == null) return;

                    foreach (var actProp in propertyActions)
                    {
                        var p = GetProperty(actProp.ExpressionProperty);
                        var pOldValue = p.GetValue(currentObs, null);
                        var pNewValue = p.GetValue(obj, null);
                        if (!pNewValue.Equals(pOldValue))
                            actProp.Action.Invoke(obj, p.Name);
                    }
                }

                var conditionActions = o.Actions.Where(w => w.ConditionExp).ToList();
                if (!conditionActions.Any()) return;

                foreach (var actCond in conditionActions)
                {
                    var func = actCond.ExpressionBool.Compile();
                    if (!func(obj)) continue;

                    var expBody = actCond.ExpressionBool.Body.ToString();
                    actCond.Action.Invoke(obj, expBody);
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
