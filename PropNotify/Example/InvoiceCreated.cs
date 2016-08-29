using System.Diagnostics;
using lib.Common;

namespace Example
{
    public class InvoiceCreated : PropNotify<Invoice>
    {
        public override void OnNotify(Invoice obj, string triggeredBy)
        {
            Debug.WriteLine($"\tPedido Gerado - ID:{obj.Id}, Valor:{obj.Payment} - Notificado: Trigger: {triggeredBy}");
        }
    }
}