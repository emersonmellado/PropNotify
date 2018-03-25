using System;
using System.Linq;
using Example;
using NUnit.Framework;
using PropNotify.Common;

namespace PropNotify.Tests
{
    [TestFixture]
    public class TrackerTests
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


            var tracker = new Tracker<Invoice>();
            tracker.Subscribe(invoiceCreated);
            tracker.Subscribe(invoiceCancelled);
            tracker.Subscribe(invoideSms);

            tracker.AddOrUpdate(new Invoice { Id = 11, Cancelled = false, Payment = 1000.0 });
            tracker.AddOrUpdate(new Invoice { Id = 22, Cancelled = false, Payment = 1000.0 });
            tracker.AddOrUpdate(new Invoice { Id = 33, Cancelled = false, Payment = 1000.0 });
            tracker.AddOrUpdate(new Invoice { Id = 11, Cancelled = true, Payment = 1020.0 });
            tracker.AddOrUpdate(new Invoice { Id = 22, Cancelled = true, Payment = 10330.0 });

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

            var tracker = new Tracker<Invoice>();
            Assert.AreEqual(0, tracker.GetObservers().Count);
            tracker.Subscribe(invoiceCreated);
            tracker.Subscribe(invoiceCancelled);
            tracker.Subscribe(invoideSms);
            Assert.AreEqual(3, tracker.GetObservers().Count);

            Assert.AreEqual(0, tracker.GetObservers().First(o => o.GetType() == typeof(InvoiceCreated)).Notifications.Count);
            Assert.AreEqual(0, tracker.GetObservers().First(o => o.GetType() == typeof(InvoiceCancelled)).Notifications.Count);
            Assert.AreEqual(0, tracker.GetObservers().First(o => o.GetType() == typeof(InvoiceSms)).Notifications.Count);

            var invoice11 = new Invoice { Id = 11, Cancelled = false, Payment = 1000.0 };

            tracker.AddOrUpdate(invoice11);
            tracker.AddOrUpdate(new Invoice { Id = 22, Cancelled = false, Payment = 1000.0 });
            tracker.AddOrUpdate(new Invoice { Id = 33, Cancelled = false, Payment = 1000.0 });

            Assert.AreEqual(3, tracker.GetList().Count);

            Assert.AreEqual(invoice11, tracker.GetObservers().First(o => o.GetType() == typeof(InvoiceCreated)).Notifications.First().Values.First(n => n.Id == 11));
            Assert.AreEqual("(p.Payment > 100)", tracker.GetObservers().First(o => o.GetType() == typeof(InvoiceCreated)).Notifications.First().Keys.First());
            Assert.AreEqual("p.Id.Equals(11)", tracker.GetObservers().First(o => o.GetType() == typeof(InvoiceCreated)).Notifications.Skip(1).First().Keys.First());

            AssertNotificationsCreated(tracker, 4);
            AssertNotificationsCancelled(tracker, 0);
            AssertNotificationsSms(tracker, 0);

            tracker.AddOrUpdate(new Invoice { Id = 11, Cancelled = true, Payment = 10.0 });

            AssertNotificationsCreated(tracker, 5);
            AssertNotificationsCancelled(tracker, 1);
            AssertNotificationsSms(tracker, 1);

            tracker.AddOrUpdate(new Invoice { Id = 22, Cancelled = true, Payment = 10330.0 });

            AssertNotificationsCreated(tracker, 6);
            AssertNotificationsCancelled(tracker, 2);
            AssertNotificationsSms(tracker, 2);
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
    }
}
