using System;
using PropNotify.Common;

namespace PropNotify
{
    public class Tracker<T> : Observable<T> where T : IEquatable<T>
    {
        public virtual void AddOrUpdate(T ped)
        {
            Notify(ped);
        }
    }

}
