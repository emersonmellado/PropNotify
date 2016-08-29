using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using lib;
using lib.Common;

namespace Example
{
    public class InvoiceCancelled : PropNotify<Invoice>
    {
        public InvoiceCancelled(params Expression<Func<Invoice, object>>[] propsInterested) : base(propsInterested)
        {
        }

        public virtual void DoNext(Invoice value, PropertyInfo propertyInfo)
        {
            Debug.WriteLine($"\tInvoice Canceled - ID:{value.Id}, Payment:{value.Payment} - Notified: Prop: {propertyInfo.Name}");
        }

        public override void OnNotify(Invoice obj, PropertyInfo propertyInfo)
        {
            DoNext(obj, propertyInfo);
        }
    }
}