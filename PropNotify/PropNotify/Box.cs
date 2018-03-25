using System;
using lib.Common;

namespace lib
{
    public class Box<T> : Observable<T> where T : IEquatable<T>
    {
        public virtual void AddOrUpdate(T ped)
        {
            Notify(ped);
        }
    }

}
