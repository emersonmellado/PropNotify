using System.Diagnostics;

namespace Example
{
    public class InvoiceSms : InvoiceCancelled
    {
        public override void DoNext(Invoice value, string triggeredBy)
        {
            Debug.WriteLine($"\tInvoice Canceled 2 - ID:{value.Id}, Payment:{value.Payment} - Notificado: Trigger: {triggeredBy}");
        }
    }
}
