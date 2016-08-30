using System;
using System.Linq.Expressions;
using lib.Common;
using lib.Interfaces;

namespace lib
{
    public class BoxFluent<T> : Observable<T> where T : IEquatable<T>
    {
        public virtual void AddOrUpdate(T obj)
        {
            Notify(obj);
        }

        public INewSubscribedClient<T> AddSub(IPropNotify<T> observer)
        {
            return new NewSubscribedClient<T>(this, observer);
        }
    }

    public interface INewSubscribedClient<T> where T : IEquatable<T>
    {
        INewNotificationType<T> AddWatchCondition(Expression<Func<T, bool>> exp);
        INewNotificationType<T> AddWatchProperty(Expression<Func<T, object>> prop);
    }

    public class NewSubscribedClient<T> : INewSubscribedClient<T>
        where T : IEquatable<T>
    {
        private readonly BoxFluent<T> _boxFluent;
        public IPropNotify<T> CurrentObserver { get; set; }

        public NewSubscribedClient(BoxFluent<T> boxFluent, IPropNotify<T> observer)
        {
            boxFluent.Subscribe(observer);
            _boxFluent = boxFluent;
            CurrentObserver = observer;
        }

        public INewNotificationType<T> AddWatchCondition(Expression<Func<T, bool>> exp)
        {
            return new NewNotificationType<T>(this, CurrentObserver, exp);
        }

        public INewNotificationType<T> AddWatchProperty(Expression<Func<T, object>> prop)
        {
            return new NewNotificationType<T>(this, CurrentObserver, prop);
        }
    }

    public class NewNotificationType<T> : INewNotificationType<T> where T : IEquatable<T>
    {
        private readonly NewSubscribedClient<T> _newSubscribedClient;
        private readonly IPropNotify<T> _currentObserver;
        private readonly Expression<Func<T, bool>> _conditional;
        private readonly Expression<Func<T, object>> _prop;
        private bool _isConditional;

        public NewNotificationType(NewSubscribedClient<T> newSubscribedClient, IPropNotify<T> currentObserver, Expression<Func<T, object>> prop)
        {
            _newSubscribedClient = newSubscribedClient;
            _currentObserver = currentObserver;
            _prop = prop;
        }

        public NewNotificationType(NewSubscribedClient<T> newSubscribedClient, IPropNotify<T> currentObserver, Expression<Func<T, bool>> conditional)
        {
            _newSubscribedClient = newSubscribedClient;
            _currentObserver = currentObserver;
            _conditional = conditional;
            _isConditional = true;
        }

        public INewSubscribedClient<T> NotifyWith(Action<T, string> action)
        {
            if (_isConditional)
            {
                _currentObserver.Actions.AddCondition(action, _conditional);
            }
            else
            {
                _currentObserver.Actions.AddProperty(action, _prop);
            }
            return _newSubscribedClient;
        }

        public INewSubscribedClient<T> Default()
        {
            return NotifyWith(_currentObserver.OnNotify);
        }
    }

    public interface INewNotificationType<T> where T : IEquatable<T>
    {
        INewSubscribedClient<T> NotifyWith(Action<T, string> action);
        INewSubscribedClient<T> Default();
    }
}
