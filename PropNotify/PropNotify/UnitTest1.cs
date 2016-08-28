using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PropNotify
{
    [TestClass]
    public class MainTestClass
    {

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
