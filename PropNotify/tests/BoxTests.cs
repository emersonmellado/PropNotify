using Example;
using lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PropNotify
{
    [TestClass]
    public class MainTestClass
    {

        [TestMethod]
        public void Sample_PropNotify()
        {
            var invoiceCreated = new InvoiceCreated(p => p.Payment, p => p.Id);
            var invoiceCancelled = new InvoiceCancelled(p => p.Cancelled);
            var invoideSms = new InvoideSms(p => p.Cancelled);


            var box = new Box<Invoice>();
            box.Subscribe(invoiceCreated);
            box.Subscribe(invoiceCancelled);
            box.Subscribe(invoideSms);

            box.AddOrUpdate(new Invoice { Id = 11, Cancelled = false, Payment = 1000.0 });
            box.AddOrUpdate(new Invoice { Id = 22, Cancelled = false, Payment = 1000.0 });
            box.AddOrUpdate(new Invoice { Id = 33, Cancelled = false, Payment = 1000.0 });
            box.AddOrUpdate(new Invoice { Id = 11, Cancelled = true, Payment = 1020.0 });
            box.AddOrUpdate(new Invoice { Id = 22, Cancelled = true, Payment = 10330.0 });
        }

    }
}
