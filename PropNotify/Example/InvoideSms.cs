using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Example
{
    public class InvoideSms : InvoiceCancelled
    {
        public InvoideSms(params Expression<Func<Invoice, object>>[] exp) : base(exp)
        {
        }

        public override void DoNext(Invoice value, PropertyInfo propertyInfo)
        {
            Debug.WriteLine($"\tInvoice Canceled 2 - ID:{value.Id}, Payment:{value.Payment} - Notificado: Prop: {propertyInfo.Name}");
        }
    }
}
