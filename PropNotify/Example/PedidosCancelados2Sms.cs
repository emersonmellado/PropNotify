using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Example
{
    public class PedidosCancelados2Sms : PedidosCancelados
    {
        public PedidosCancelados2Sms(params Expression<Func<Invoice, object>>[] exp) : base(exp)
        {
        }

        public override void DoNext(Invoice value, PropertyInfo propertyInfo)
        {
            Debug.WriteLine($"\tPedido Cancelado 2 - ID:{value.Id}, Valor:{value.Valor} - Notificado: Prop: {propertyInfo.Name}");
        }
    }
}
