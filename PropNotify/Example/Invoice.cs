namespace Example
{
    public class Invoice
    {
        public long Id { get; set; }
        public double Payment { get; set; }
        public bool Cancelled { get; set; }

        public override bool Equals(object obj)
        {
            var pCast = (Invoice)obj;
            return Id.Equals(pCast.Id);
        }

        protected bool Equals(Invoice other)
        {
            return Id == other.Id && Payment.Equals(other.Payment) && Cancelled == other.Cancelled;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ Payment.GetHashCode();
                hashCode = (hashCode * 397) ^ Cancelled.GetHashCode();
                return hashCode;
            }
        }
    }
}
