using System.Diagnostics;
using lib.Common;

namespace Example
{
    public class InvoiceCreated : PropNotify<Invoice>
    {
        public override void OnNotify(Invoice obj, string triggeredBy)
        {
            Debug.WriteLine($"\tInvoice Created - ID:{obj.Id}, Payment:{obj.Payment} - Notified: Trigger: {triggeredBy}");
        }
    }
}