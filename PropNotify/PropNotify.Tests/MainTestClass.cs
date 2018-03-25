using System;
using System.Diagnostics;
using System.Linq;
using Example;
using NUnit.Framework;
using PropNotify.Common;

namespace PropNotify.Tests
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
            //Each condition will generate a notifications so 2 for Created every call that hits
            invoiceCreated.AddWatchCondition(p => p.Payment > 100, p => p.Id.Equals(11));

            var invoiceCancelled = new InvoiceCancelled();
            invoiceCancelled.AddWatchCondition(p => p.Cancelled && p.Id > 0);

            var invoideSms = new InvoiceSms();
            invoideSms.AddWatchCondition(p => p.Cancelled && p.Id > 0);

            var box = new Box<Invoice>();
            Assert.AreEqual(0, box.GetObservers().Count);
            box.Subscribe(invoiceCreated);
            box.Subscribe(invoiceCancelled);
            box.Subscribe(invoideSms);
            Assert.AreEqual(3, box.GetObservers().Count);

            Assert.AreEqual(0, box.GetObservers().First(o => o.GetType() == typeof(InvoiceCreated)).Notifications.Count);
            Assert.AreEqual(0, box.GetObservers().First(o => o.GetType() == typeof(InvoiceCancelled)).Notifications.Count);
            Assert.AreEqual(0, box.GetObservers().First(o => o.GetType() == typeof(InvoiceSms)).Notifications.Count);

            var invoice11 = new Invoice { Id = 11, Cancelled = false, Payment = 1000.0 };

            box.AddOrUpdate(invoice11);
            box.AddOrUpdate(new Invoice { Id = 22, Cancelled = false, Payment = 1000.0 });
            box.AddOrUpdate(new Invoice { Id = 33, Cancelled = false, Payment = 1000.0 });

            Assert.AreEqual(3, box.GetList().Count);

            Assert.AreEqual(invoice11, box.GetObservers().First(o => o.GetType() == typeof(InvoiceCreated)).Notifications.First().Values.First(n => n.Id == 11));
            Assert.AreEqual("(p.Payment > 100)", box.GetObservers().First(o => o.GetType() == typeof(InvoiceCreated)).Notifications.First().Keys.First());
            Assert.AreEqual("p.Id.Equals(11)", box.GetObservers().First(o => o.GetType() == typeof(InvoiceCreated)).Notifications.Skip(1).First().Keys.First());

            AssertNotificationsCreated(box, 4);
            AssertNotificationsCancelled(box, 0);
            AssertNotificationsSms(box, 0);

            box.AddOrUpdate(new Invoice { Id = 11, Cancelled = true, Payment = 10.0 });

            AssertNotificationsCreated(box, 5);
            AssertNotificationsCancelled(box, 1);
            AssertNotificationsSms(box, 1);

            box.AddOrUpdate(new Invoice { Id = 22, Cancelled = true, Payment = 10330.0 });

            AssertNotificationsCreated(box, 6);
            AssertNotificationsCancelled(box, 2);
            AssertNotificationsSms(box, 2);
        }

        private static void AssertNotificationsSms(Observable<Invoice> box, int occurrences)
        {
            AssertNotifications(box, typeof(InvoiceSms), occurrences);
        }

        private static void AssertNotificationsCreated(Observable<Invoice> box, int occurrences)
        {
            AssertNotifications(box, typeof(InvoiceCreated), occurrences);
        }

        private static void AssertNotificationsCancelled(Observable<Invoice> box, int occurrences)
        {
            AssertNotifications(box, typeof(InvoiceCancelled), occurrences);
        }

        private static void AssertNotifications(Observable<Invoice> box, Type type, int occurrences)
        {
            Assert.AreEqual(occurrences, box.GetObservers().First(o => o.GetType() == type).Notifications.Count);
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
