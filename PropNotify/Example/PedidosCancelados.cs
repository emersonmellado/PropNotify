using lib.ObservablePattern;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Example
{
    public class PedidosCancelados : ObsNotify<Invoice>
    {
        public PedidosCancelados(params Expression<Func<Invoice, object>>[] propsInterested) : base(propsInterested)
        {
        }

        public virtual void DoNext(Invoice value, PropertyInfo propertyInfo)
        {
            Debug.WriteLine($"\tPedido Cancelado - ID:{value.Id}, Valor:{value.Valor} - Notificado: Prop: {propertyInfo.Name}");
        }

        public override void OnNotify(Invoice mod, PropertyInfo propertyInfo)
        {
            DoNext(mod, propertyInfo);
        }
    }
}