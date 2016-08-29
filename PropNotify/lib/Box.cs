using lib.Common;

namespace lib
{
    public class Box<T> : Observable<T>
    {
        public virtual void AddOrUpdate(T ped)
        {
            Notify(ped);
        }
    }

}
