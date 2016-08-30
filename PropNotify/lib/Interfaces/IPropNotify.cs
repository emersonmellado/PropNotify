using System;
using System.Collections.Generic;
using lib.Common;

namespace lib.Interfaces
{
    public interface IPropNotify<T> where T : IEquatable<T>
    {
        void OnNotify(T obj, string triggeredBy);
        List<ActionHolder<T>> Actions { get; set; }
    }
}
