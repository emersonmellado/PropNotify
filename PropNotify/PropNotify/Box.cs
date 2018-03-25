using System;
using PropNotify.Common;

namespace PropNotify
{
    public class Box<T> : Observable<T> where T : IEquatable<T>
    {
        public virtual void AddOrUpdate(T ped)
        {
            Notify(ped);
        }
    }

}
