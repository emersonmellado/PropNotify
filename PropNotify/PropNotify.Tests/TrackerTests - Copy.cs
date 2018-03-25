using System.Diagnostics;
using Example;
using NUnit.Framework;

namespace PropNotify.Tests
{
    [TestFixture]
    public class TrackerFluentTests
    {
        [Test]
        public void Sample_FluentObserver_PropNotify()
        {
            var trackerFluent = new TrackerFluent<Invoice>();
            trackerFluent.AddSub(new InvoiceCreated())
                .AddWatchCondition(p => p.Id.Equals(11)).Default()
                .AddWatchCondition(p => p.Id.Equals(11))
                .NotifyWith((a, trigger) =>
                {
                    Debug.WriteLine($"\t[RULE ID11] Custom Notify 1: ID: {a.Id} Payment: {a.Payment} - Trigger:{trigger}");
                });

            trackerFluent.AddOrUpdate(new Invoice { Id = 11, Cancelled = false, Payment = 1000.0 });
            trackerFluent.AddOrUpdate(new Invoice { Id = 22, Cancelled = false, Payment = 1000.0 });
            trackerFluent.AddOrUpdate(new Invoice { Id = 33, Cancelled = false, Payment = 1000.0 });
            trackerFluent.AddOrUpdate(new Invoice { Id = 11, Cancelled = true, Payment = 1020.0 });
            trackerFluent.AddOrUpdate(new Invoice { Id = 22, Cancelled = true, Payment = 10330.0 });

            //todo: add asserts!
        }

    }
}
