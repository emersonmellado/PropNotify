using PropNotify.Common;

namespace PropNotify.Sample
{
    public class InvoiceCancelled : PropNotify<Invoice>
    {
        public virtual void DoSomethingNext(Invoice obj, string trigger)
        {
            //AddNotification($"\tInvoice Canceled - ID:{value.Id}, Payment:{value.Payment} - Notified: Trigger: {triggeredBy}");
            AddNotification(trigger, obj);
        }

        public override void OnNotify(Invoice obj, string triggeredBy)
        {
            DoSomethingNext(obj, triggeredBy);
        }
    }
}