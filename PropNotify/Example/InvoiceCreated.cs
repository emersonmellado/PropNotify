using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using lib;
using lib.Common;

namespace Example
{
    public class InvoiceCreated : PropNotify<Invoice>
    {
        public InvoiceCreated(params Expression<Func<Invoice, object>>[] propsInterested)
            : base(propsInterested)
        {
        }
        public override void OnNotify(Invoice obj, PropertyInfo propertyInfo)
        {
            Debug.WriteLine($"\tPedido Gerado - ID:{obj.Id}, Valor:{obj.Payment} - Notificado: Prop:{propertyInfo.Name}");
        }
    }
}