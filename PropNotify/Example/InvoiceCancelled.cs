using System.Diagnostics;
using System.Reflection;
using lib.Common;

namespace Example
{
    public class InvoiceCancelled : PropNotify<Invoice>
    {
        public virtual void DoNext(Invoice value, string triggeredBy)
        {
            Debug.WriteLine($"\tInvoice Canceled - ID:{value.Id}, Payment:{value.Payment} - Notified: Trigger: {triggeredBy}");
        }

        public override void OnNotify(Invoice obj, string triggeredBy)
        {
            DoNext(obj, triggeredBy);
        }
    }
}