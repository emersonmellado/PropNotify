using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Example
{
    public class PedidosCancelados2Sms : PedidosCancelados
    {
        public PedidosCancelados2Sms(params Expression<Func<Pedido, object>>[] exp) : base(exp)
        {
        }

        public override void DoNext(Pedido value, PropertyInfo propertyInfo)
        {
            Debug.WriteLine($"\tPedido Cancelado 2 - ID:{value.Id}, Valor:{value.Valor} - Notificado: Prop: {propertyInfo.Name}");
        }
    }
}
