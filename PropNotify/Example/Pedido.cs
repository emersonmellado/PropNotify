namespace Example
{
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

        protected bool Equals(Pedido other)
        {
            return Id == other.Id && Valor.Equals(other.Valor) && Cancelado == other.Cancelado;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ Valor.GetHashCode();
                hashCode = (hashCode * 397) ^ Cancelado.GetHashCode();
                return hashCode;
            }
        }
    }
}
