using Example;
using lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PropNotify
{
    [TestClass]
    public class MainTestClass
    {

        [TestMethod]
        public void Sample_PropToObserver_PropNotify()
        {
            var invoiceCreated = new InvoiceCreated();
            invoiceCreated.AddPropertiesToObserver(p => p.Payment, p => p.Id);

            var invoiceCancelled = new InvoiceCancelled();
            invoiceCancelled.AddPropertiesToObserver(p => p.Cancelled);

            var invoideSms = new InvoiceSms();
            invoideSms.AddPropertiesToObserver(p => p.Cancelled);


            var box = new Box<Invoice>();
            box.Subscribe(invoiceCreated);
            box.Subscribe(invoiceCancelled);
            box.Subscribe(invoideSms);

            box.AddOrUpdate(new Invoice { Id = 11, Cancelled = false, Payment = 1000.0 });
            box.AddOrUpdate(new Invoice { Id = 22, Cancelled = false, Payment = 1000.0 });
            box.AddOrUpdate(new Invoice { Id = 33, Cancelled = false, Payment = 1000.0 });
            box.AddOrUpdate(new Invoice { Id = 11, Cancelled = true, Payment = 1020.0 });
            box.AddOrUpdate(new Invoice { Id = 22, Cancelled = true, Payment = 10330.0 });

            //todo: add asserts!

        }

        [TestMethod]
        public void Sample_ConditionToObserver_PropNotify()
        {
            var invoiceCreated = new InvoiceCreated();
            invoiceCreated.AddConditionToObserver(p => p.Payment > 100, p => p.Id.Equals(11));

            var invoiceCancelled = new InvoiceCancelled();
            invoiceCancelled.AddConditionToObserver(p => p.Cancelled && p.Id > 0);

            var invoideSms = new InvoiceSms();
            invoideSms.AddConditionToObserver(p => p.Cancelled && p.Id > 0);

            var box = new Box<Invoice>();
            box.Subscribe(invoiceCreated);
            box.Subscribe(invoiceCancelled);
            box.Subscribe(invoideSms);

            box.AddOrUpdate(new Invoice { Id = 11, Cancelled = false, Payment = 1000.0 });
            box.AddOrUpdate(new Invoice { Id = 22, Cancelled = false, Payment = 1000.0 });
            box.AddOrUpdate(new Invoice { Id = 33, Cancelled = false, Payment = 1000.0 });
            box.AddOrUpdate(new Invoice { Id = 11, Cancelled = true, Payment = 1020.0 });
            box.AddOrUpdate(new Invoice { Id = 22, Cancelled = true, Payment = 10330.0 });

            //todo: add asserts!
        }

    }
}
