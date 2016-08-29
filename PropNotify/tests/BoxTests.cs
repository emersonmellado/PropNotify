using Example;
using lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
}
