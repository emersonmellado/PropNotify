using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using lib.ObservablePattern;

namespace Example
{
    public class PedidosCancelados : ObsNotify<Pedido>
    {
        public PedidosCancelados(params Expression<Func<Pedido, object>>[] propsInterested) : base(propsInterested)
        {
        }

        public virtual void DoNext(Pedido value, PropertyInfo propertyInfo)
        {
            Debug.WriteLine($"\tPedido Cancelado - ID:{value.Id}, Valor:{value.Valor} - Notificado: Prop: {propertyInfo.Name}");
        }

        public override void OnNotify(Pedido mod, PropertyInfo propertyInfo)
        {
            DoNext(mod, propertyInfo);
        }
    }
}