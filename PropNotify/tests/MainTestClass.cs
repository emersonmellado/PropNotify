using System.Diagnostics;
using Example;
using lib;
using NUnit.Framework;

namespace PropNotify
{
    [TestFixture]
    public class MainTestClass
    {

        [Test]
        public void Sample_PropToObserver_PropNotify()
        {
            var invoiceCreated = new InvoiceCreated();
            invoiceCreated.AddWatchProperty(p => p.Payment, p => p.Id);

            var invoiceCancelled = new InvoiceCancelled();
            invoiceCancelled.AddWatchProperty(p => p.Cancelled);

            var invoideSms = new InvoiceSms();
            invoideSms.AddWatchProperty(p => p.Cancelled);


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

        [Test]
        public void Sample_ConditionToObserver_PropNotify()
        {
            var invoiceCreated = new InvoiceCreated();
            invoiceCreated.AddWatchCondition(p => p.Payment > 100, p => p.Id.Equals(11));

            var invoiceCancelled = new InvoiceCancelled();
            invoiceCancelled.AddWatchCondition(p => p.Cancelled && p.Id > 0);

            var invoideSms = new InvoiceSms();
            invoideSms.AddWatchCondition(p => p.Cancelled && p.Id > 0);

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

        [Test]
        public void Sample_FluentObserver_PropNotify()
        {
            var box = new BoxFluent<Invoice>();
            box.AddSub(new InvoiceCreated())
                .AddWatchCondition(p => p.Id.Equals(11)).Default()
                .AddWatchCondition(p => p.Id.Equals(11))
                .NotifyWith((a, trigger) =>
                {
                    Debug.WriteLine($"\t[RULE ID11] Custom Notify 1: ID: {a.Id} Payment: {a.Payment} - Trigger:{trigger}");
                });

            box.AddOrUpdate(new Invoice { Id = 11, Cancelled = false, Payment = 1000.0 });
            box.AddOrUpdate(new Invoice { Id = 22, Cancelled = false, Payment = 1000.0 });
            box.AddOrUpdate(new Invoice { Id = 33, Cancelled = false, Payment = 1000.0 });
            box.AddOrUpdate(new Invoice { Id = 11, Cancelled = true, Payment = 1020.0 });
            box.AddOrUpdate(new Invoice { Id = 22, Cancelled = true, Payment = 10330.0 });

            //todo: add asserts!
        }

    }
}
