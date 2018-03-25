using System.Diagnostics;
using System.Net.Mail;
using lib.Common;

namespace Example
{
    public class InvoiceCreated : PropNotify<Invoice>
    {
        public override void OnNotify(Invoice obj, string trigger)
        {
            //$"\tInvoice Created - ID:{obj.Id}, Payment:{obj.Payment} - Notified: Trigger: {triggeredBy}"
            AddNotification(trigger, obj);
        }
    }
}