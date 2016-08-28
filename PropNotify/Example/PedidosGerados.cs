using lib.ObservablePattern;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Example
{
    public class PedidosGerados : ObsNotify<Pedido>
    {
        public PedidosGerados(params Expression<Func<Pedido, object>>[] propsInterested)
            : base(propsInterested)
        {
        }
        public override void OnNotify(Pedido mod, PropertyInfo propertyInfo)
        {
            Debug.WriteLine($"\tPedido Gerado - ID:{mod.Id}, Valor:{mod.Valor} - Notificado: Prop:{propertyInfo.Name}");
        }
    }
}