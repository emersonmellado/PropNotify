using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ThermometerLibrary;
using ThermometerLibrary.ObservablePattern;

namespace ThermometerUnitTest
{
    [TestClass]
    public class MainTestClass
    {
        private TemperatureHandler _pub;
        private Checker _checker;
        private double[] _data;

        [TestMethod]
        public void Handler__Water_With_4_Points__ShouldNotify_2_Times()
        {
            _data = new[] { 100.0, 105, 115, 120 };
            _pub = new TemperatureHandler(_data);
            _checker = new Checker("Water", 100, 0, 10);
            Assert.AreEqual("Water", _checker.GetName());

            _pub.AddListener(_checker);
            _pub.Start();

            const int expectedNotifications = 2;
            Assert.AreEqual(expectedNotifications, _checker.NotifiedTempList.Count);
            Assert.AreEqual(100, _checker.NotifiedTempList[0]);
            Assert.AreEqual(115, _checker.NotifiedTempList[1]);

            PrintDebugTestNumbers();
        }

        [TestMethod]
        public void Handler_Corn_With_7_Points__ShouldNotify_4_Times()
        {
            _data = new[] { 12.56, 56.65, -22, 247, 250, 265, 290 };
            _pub = new TemperatureHandler(_data);
            _checker = new Checker("Corn", 246, 0, 10);
            Assert.AreEqual("Corn", _checker.GetName());

            _pub.AddListener(_checker);
            _pub.Start();

            const int expectedNotifications = 4;
            Assert.AreEqual(expectedNotifications, _checker.NotifiedTempList.Count);
            Assert.AreEqual(-22, _checker.NotifiedTempList[0]);
            Assert.AreEqual(247, _checker.NotifiedTempList[1]);
            Assert.AreEqual(265, _checker.NotifiedTempList[2]);
            Assert.AreEqual(290, _checker.NotifiedTempList[3]);

            PrintDebugTestNumbers();
        }

        [TestMethod]
        public void Observable_Corn_With_7_Points__ShouldNotify_4_Times()
        {
            _checker = new Checker("Corn", 246, 0, 10);
            var dataHolder = new DataTemperatureHolder();
            dataHolder.Subscribe(_checker);

            _data = new[] { -22, 247.0, 250, 265, 290 };
            Array.ForEach(_data, d => dataHolder.AddNewTemp(d));
            const int expectedNotifications = 4;
            Assert.AreEqual(expectedNotifications, _checker.NotifiedTempList.Count);
            Assert.AreEqual(-22, _checker.NotifiedTempList[0]);
            Assert.AreEqual(247, _checker.NotifiedTempList[1]);
            Assert.AreEqual(265, _checker.NotifiedTempList[2]);
            Assert.AreEqual(290, _checker.NotifiedTempList[3]);
        }


        [TestMethod]
        public void Sample_Obsaverble()
        {
            var pedGerados = new PedidosGerados(p => p.Valor, p => p.Id);
            var pedCancelados = new PedidosCancelados(p => p.Cancelado);
            var pedCancelados2Sms = new PedidosCancelados2Sms(p => p.Cancelado);


            var pedidos = new Box<Pedido>();
            pedidos.Subscribe(pedGerados);
            pedidos.Subscribe(pedCancelados);
            pedidos.Subscribe(pedCancelados2Sms);

            pedidos.AddOrUpdate(new Pedido { Id = 11, Cancelado = false, Valor = 1000.0 });
            pedidos.AddOrUpdate(new Pedido { Id = 22, Cancelado = false, Valor = 1000.0 });
            pedidos.AddOrUpdate(new Pedido { Id = 33, Cancelado = false, Valor = 1000.0 });
            pedidos.AddOrUpdate(new Pedido { Id = 11, Cancelado = true, Valor = 1020.0 });
            pedidos.AddOrUpdate(new Pedido { Id = 22, Cancelado = true, Valor = 10330.0 });
        }



        private void PrintDebugTestNumbers()
        {
            var numbers = _checker.NotifiedTempList.Select(s => s.ToString(CultureInfo.InvariantCulture)).Aggregate((a, b) => a + "||" + b);

            Debug.WriteLine($"Data List: {_data.Select(s => s.ToString(CultureInfo.InvariantCulture)).Aggregate((a, b) => a + "||" + b)}");
            Debug.WriteLine(_checker.NotifiedTempList.Count, $"Returned {numbers.Count()} times! - X:{numbers}");
        }
    }

    public class PedidosGerados : ObsNotify<Pedido>
    {
        public PedidosGerados(params Expression<Func<Pedido, object>>[] propsInterested)
            : base(propsInterested)
        {
        }
        public override void OnNotify(Pedido mod, PropertyInfo propertyInfo)
        {
            Debug.WriteLine($"\tPedido Gerado - ID:{mod.Id}, Valor:{mod.Valor} - Notificado: Prop:{propertyInfo.Name}");
        }
    }

    public class PedidosCancelados : ObsNotify<Pedido>
    {
        public PedidosCancelados(params Expression<Func<Pedido, object>>[] propsInterested) : base(propsInterested)
        {
        }

        public virtual void DoNext(Pedido value, PropertyInfo propertyInfo)
        {
            Debug.WriteLine($"\tPedido Cancelado - ID:{value.Id}, Valor:{value.Valor} - Notificado: Prop: {propertyInfo.Name}");
        }

        public override void OnNotify(Pedido mod, PropertyInfo propertyInfo)
        {
            DoNext(mod, propertyInfo);
        }
    }

    public class PedidosCancelados2Sms : PedidosCancelados
    {
        public PedidosCancelados2Sms(params Expression<Func<Pedido, object>>[] exp) : base(exp)
        {
        }

        public override void DoNext(Pedido value, PropertyInfo propertyInfo)
        {
            Debug.WriteLine($"\tPedido Cancelado 2 - ID:{value.Id}, Valor:{value.Valor} - Notificado: Prop: {propertyInfo.Name}");
        }
    }

    public class Box<T> : Observable<T>
    {
        public virtual void AddOrUpdate(T ped)
        {
            Notify(ped);
        }
    }

    public class Pedido
    {
        public long Id { get; set; }
        public double Valor { get; set; }
        public bool Cancelado { get; set; }

        public override bool Equals(object obj)
        {
            var pCast = (Pedido)obj;
            return Id.Equals(pCast.Id);
        }
    }




}
