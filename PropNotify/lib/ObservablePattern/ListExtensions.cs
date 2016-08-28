using System.Collections.Generic;
using System.Linq;

namespace ThermometerLibrary.ObservablePattern
{
    public static class ListExtension
    {
        public static void AddOrUpdate<T>(this List<T> lista, T newItem)
        {

            if (lista.Any(a => a.Equals(newItem)))
                lista.Remove(newItem);

            lista.Add(newItem);
        }
    }
}
