namespace Example
{
    public class InvoiceSms : InvoiceCancelled
    {
        public override void DoSomethingNext(Invoice obj, string trigger)
        {
            //AddNotification($"\tInvoice Canceled 2 - ID:{value.Id}, Payment:{value.Payment} - Notificado: Trigger: {triggeredBy}");
            AddNotification(trigger, obj);
        }
    }
}
